// File: Services/IStudentService.cs

using StudentManagementAPI.Models;

namespace StudentManagementAPI.Services
{
    public interface IStudentService
    {
        // Notice: Different names and responsibilities than Repository!
        List<Student> GetAllStudents();           // ← High-level operation
        Student?GetStudentById(int id);
        Student CreateStudent(CreateStudentDto dto);  // ← Takes DTO, not Student
        bool DeleteStudent(int id);
    }
    
    // DTO = Data Transfer Object
    // Used to receive data from client without exposing internal model
    public class CreateStudentDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        // Notice: No ID (auto-generated), No CreatedAt (auto-set by service)
    }
}