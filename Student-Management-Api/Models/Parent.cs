namespace StudentManagementAPI.Models
{
    public class Parent
    {
        // Primary Key
        public int Id { get; set; }
        
        // Parent Properties
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Occupation { get; set; }
        public string? Relationship { get; set; }  // Father, Mother, Guardian
        
        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // ===== NEW: Navigation Property =====
        // One-to-Many: Parent has many Students
        public ICollection<Student> Students { get; set; } = new List<Student>();
        // Access all students of this parent
    }
}
