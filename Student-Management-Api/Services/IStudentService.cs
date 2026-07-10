using StudentManagementAPI.Models;

namespace StudentManagementAPI.Services
{
    public interface IStudentService
    {
        List<Student> GetAllStudents();
        Student? GetStudentById(int id);
        Student? GetStudentWithParent(int id);
        StudentWithParentDto? GetStudentWithParentDto(int id);  // NEW!
        Student CreateStudent(CreateStudentDto dto);
        Student? UpdateStudent(int id, UpdateStudentDto dto);
        bool DeleteStudent(int id);
    }
}
