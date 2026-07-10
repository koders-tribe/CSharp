namespace StudentManagementAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? RollNumber { get; set; }
        public int Grade { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public int? ParentId { get; set; }  // ← Changed to nullable!
        
        public Parent? Parent { get; set; }
        public ICollection<StudentTeacher> StudentTeachers { get; set; } = new List<StudentTeacher>();
    }
}