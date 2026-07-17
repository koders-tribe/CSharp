using StudentManagementAPI.Services.Dtos.Auth;

namespace StudentManagementAPI.Services.Dtos.Auth
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public required string Token { get; set; }
        public UserDto? User { get; set; }
    }
}