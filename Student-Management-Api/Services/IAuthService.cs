using StudentManagementAPI.Services.Dtos.Auth;

namespace StudentManagementAPI.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message, string? token)> RegisterAsync(RegisterDto dto);
        Task<(bool success, string message, string? token, UserDto? user)> LoginAsync(LoginDto dto);
        Task<UserDto?> GetUserByIdAsync(int id);
    }
}