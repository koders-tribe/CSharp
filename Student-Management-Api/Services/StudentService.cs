using StudentManagementAPI.Data;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Services
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

    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        // Get all students
        public List<Student> GetAllStudents()
        {
            return _repository.GetAll();
        }

        // Get student by ID
        public Student? GetStudentById(int id)
        {
            return _repository.GetById(id);
        }

        // Get student with parent
        public Student? GetStudentWithParent(int id)
        {
            return _repository.GetByIdWithParent(id);
        }

        // Get student with parent as DTO (no circular reference)
        public StudentWithParentDto? GetStudentWithParentDto(int id)
        {
            var student = _repository.GetByIdWithParent(id);
            if (student == null)
                return null;

            return new StudentWithParentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                RollNumber = student.RollNumber,
                Grade = student.Grade,
                DateOfBirth = student.DateOfBirth,
                ParentId = student.ParentId,
                CreatedAt = student.CreatedAt,
                UpdatedAt = student.UpdatedAt,
                Parent = student.Parent == null ? null : new ParentDto
                {
                    Id = student.Parent.Id,
                    Name = student.Parent.Name,
                    Email = student.Parent.Email,
                    Phone = student.Parent.Phone,
                    Occupation = student.Parent.Occupation,
                    Relationship = student.Parent.Relationship,
                    CreatedAt = student.Parent.CreatedAt,
                    UpdatedAt = student.Parent.UpdatedAt
                }
            };
        }

        // Create student
        public Student CreateStudent(CreateStudentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Student name is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Student email is required");

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                RollNumber = dto.RollNumber,
                Grade = dto.Grade,
                DateOfBirth = dto.DateOfBirth,
                ParentId = dto.ParentId
            };

            return _repository.Add(student);
        }

        // Update student
        public Student? UpdateStudent(int id, UpdateStudentDto dto)
        {
            var student = GetStudentById(id);
            if (student == null)
                return null;

            var updatedStudent = new Student
            {
                Id = student.Id,
                Name = dto.Name ?? student.Name,
                Email = dto.Email ?? student.Email,
                Phone = dto.Phone ?? student.Phone,
                RollNumber = dto.RollNumber ?? student.RollNumber,
                Grade = dto.Grade > 0 ? dto.Grade : student.Grade,
                DateOfBirth = dto.DateOfBirth != default ? dto.DateOfBirth : student.DateOfBirth,
                ParentId = dto.ParentId ?? student.ParentId
            };

            return _repository.Update(id, updatedStudent);
        }

        // Delete student
        public bool DeleteStudent(int id)
        {
            return _repository.Delete(id);
        }
    }
}
