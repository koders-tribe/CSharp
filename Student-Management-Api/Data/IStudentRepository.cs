// File: Data/IStudentRepository.cs

using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public interface IStudentRepository   // here the interface defines the contract for student repository implementations{what method exist in the repository}
    {
        List<Student> GetAll();           // ← What methods must exist?
        Student? GetById(int id);
        Student? GetByIdWithParent(int id);          // ← All implementations MUST have these
        Student Add(Student student);
        Student? Update(int id, Student student); //
        bool Delete(int id);
    }
}