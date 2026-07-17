using BCrypt.Net;
using StudentManagementAPI.Data;
using StudentManagementAPI.Models;
using StudentManagementAPI.Services.Dtos.Auth;

namespace StudentManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

       public async Task<(bool success, string message, string? token)> RegisterAsync(RegisterDto dto)
{
    // Validate input
    if (string.IsNullOrWhiteSpace(dto.Username))
        return (false, "Username is required", null);

    if (string.IsNullOrWhiteSpace(dto.Password))
        return (false, "Password is required", null);

    if (string.IsNullOrWhiteSpace(dto.Email))
        return (false, "Email is required", null);

    if (string.IsNullOrWhiteSpace(dto.Role))
        return (false, "Role is required", null);

    // Check if user already exists
    bool userExists = await _userRepository.UserExistsAsync(dto.Username);
    if (userExists)
        return (false, "Username already taken", null);

    // Validate role
    var validRoles = new[] { "Admin", "Teacher", "Student" };
    if (!validRoles.Contains(dto.Role))
        return (false, "Invalid role. Must be Admin, Teacher, or Student", null);

    // ✅ NEW VALIDATION: If role is Student, StudentId is required
    if (dto.Role == "Student" && !dto.StudentId.HasValue)
        return (false, "StudentId is required for Student role", null);

    // ✅ NEW VALIDATION: If role is NOT Student, StudentId must be null
    if (dto.Role != "Student" && dto.StudentId.HasValue)
        return (false, "StudentId should only be provided for Student role", null);

    // Create user
    var user = new User
    {
        Username = dto.Username,
        Email = dto.Email,
        Role = dto.Role,
        StudentId = dto.StudentId,  // Will be null for Admin/Teacher
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        CreatedAt = DateTime.UtcNow
    };

    // Save to database
    await _userRepository.AddAsync(user);
    bool saved = await _userRepository.SaveChangesAsync();

    if (!saved)
        return (false, "Failed to register user", null);

    // Generate token
    string token = _jwtService.GenerateToken(user);

    return (true, "User registered successfully", token);
}
        public async Task<(bool success, string message, string? token, UserDto? user)> LoginAsync(LoginDto dto)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(dto.Username))
                return (false, "Username is required", null, null);

            if (string.IsNullOrWhiteSpace(dto.Password))
                return (false, "Password is required", null, null);

            // Find user
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
                return (false, "Invalid username or password", null, null);

            // Verify password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isPasswordValid)
                return (false, "Invalid username or password", null, null);

            // Generate token
            string token = _jwtService.GenerateToken(user);

            // Convert to DTO
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                StudentId = user.StudentId
            };

            return (true, "Login successful", token, userDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                StudentId = user.StudentId
            };
        }
    }
}