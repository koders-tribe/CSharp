namespace StudentManagementAPI.Services.Dtos
{
    // DTO for creating parent
    public class CreateParentDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Occupation { get; set; }
        public string? Relationship { get; set; }
    }

    // DTO for updating parent
    public class UpdateParentDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Occupation { get; set; }
        public string? Relationship { get; set; }
    }

    // DTO: Parent (simple, no Students collection)
    public class ParentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Occupation { get; set; }
        public string? Relationship { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}