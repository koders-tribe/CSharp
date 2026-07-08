namespace StudentManagementAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Subject { get; set; }
        public string? Qualification { get; set; }
        public int Experience { get; set; }
        public decimal Salary { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}