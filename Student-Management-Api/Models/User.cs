using System;

namespace StudentManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; } // "Admin", "Teacher", "Student"
        public int? StudentId { get; set; } // Optional: if role is Student
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property (optional)
        public Student ? Student { get; set; }
    }
}