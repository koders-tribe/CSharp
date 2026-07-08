namespace StudentManagementAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }        // ← Add required
        public required string Email { get; set; }       // ← Add required
        public string? Phone { get; set; }               // Optional
        public DateTime CreatedAt { get; set; }
    }
}