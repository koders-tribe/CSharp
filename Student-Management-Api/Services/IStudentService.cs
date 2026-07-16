using StudentManagementAPI.Models;
using StudentManagementAPI.Services.Dtos;

namespace StudentManagementAPI.Services
{
    public interface IStudentService
    {
        // Existing methods
        List<Student> GetAllStudents();
        Student? GetStudentById(int id);
        Student? GetStudentWithParent(int id);
        StudentWithParentDto? GetStudentWithParentDto(int id);
        Student CreateStudent(CreateStudentDto dto);
        Student? UpdateStudent(int id, UpdateStudentDto dto);
        bool DeleteStudent(int id);

        // NEW: Day 5 Pagination & Filtering Methods
        
        /// <summary>
        /// Get paginated, filtered students with complete response
        /// </summary>
        Task<PaginatedResponse<StudentDto>> GetStudentsPaginatedAsync(
            StudentFilterDto filter);

        /// <summary>
        /// Simple search students by name/email
        /// </summary>
        Task<List<StudentDto>> SearchStudentsAsync(string search);

        /// <summary>
        /// Get students by grade
        /// </summary>
        Task<List<StudentDto>> GetStudentsByGradeAsync(int grade);
    }
}