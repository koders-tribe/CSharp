namespace StudentManagementAPI.Services.Dtos.Auth
{
    public class RegisterDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; } // "Admin", "Teacher", "Student"
        public int? StudentId { get; set; } // Optional: if role is Student
    }
}