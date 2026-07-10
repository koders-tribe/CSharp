
namespace StudentManagementAPI.Models
{
    public class Teacher
    {
        // Primary Key
        public int Id { get; set; }
        
        // Teacher Properties
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Subject { get; set; }
        public string? Qualification { get; set; }
        public int Experience { get; set; }
        public decimal Salary { get; set; }
        public DateTime DateOfJoining { get; set; }
        
        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // ===== NEW: Navigation Property =====
        // Many-to-Many: Teacher has many Students
        public ICollection<StudentTeacher> StudentTeachers { get; set; } 
            = new List<StudentTeacher>();
        // Access all students of this teacher
    }
}