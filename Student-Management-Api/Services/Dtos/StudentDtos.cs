namespace StudentManagementAPI.Services.Dtos
{
    // DTO for creating student
    public class CreateStudentDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? RollNumber { get; set; }
        public int Grade { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? ParentId { get; set; }
    }

    // DTO for updating student
    public class UpdateStudentDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? RollNumber { get; set; }
        public int Grade { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? ParentId { get; set; }
    }

    // DTO: Student response (used in pagination/search)
    public class StudentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? RollNumber { get; set; }
        public int Grade { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ParentDto? Parent { get; set; }
    }

    // DTO: Student with Parent (avoids circular reference)
    public class StudentWithParentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? RollNumber { get; set; }
        public int Grade { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parent data (no circular ref - Parent doesn't have Students collection)
        public ParentDto? Parent { get; set; }
    }
}