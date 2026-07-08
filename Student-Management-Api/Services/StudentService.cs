// File: Services/StudentService.cs

using StudentManagementAPI.Data;      // ← Uses Repository interface
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        
        // ← KEY: Repository is INJECTED via constructor
        // This is Dependency Injection!
        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }
        
        public List<Student> GetAllStudents()
        {
            // Just delegates to repository
            return _repository.GetAll();
        }
        
        public Student? GetStudentById(int id)
        {
            // Just delegates to repository
            return _repository.GetById(id);
        }
        
        public Student CreateStudent(CreateStudentDto dto)
        {
            // ← HERE'S THE BUSINESS LOGIC!
            
            // Validation 1: Name is required
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required");
            
            // Validation 2: Email is required
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required");
            
            // Create Student object from DTO
            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
                // ID and CreatedAt are set by repository
            };
            
            // Persist to repository
            return _repository.Add(student);
        }
        
        public bool DeleteStudent(int id)
        {
            // Just delegates to repository
            return _repository.Delete(id);
        }
    }
}