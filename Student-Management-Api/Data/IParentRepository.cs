using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public interface IParentRepository
    {
        List<Parent> GetAll();
        Parent? GetByIdWithStudents(int id);  // ← New method to get parent with associated students
        Parent? GetById(int id);
        Parent Add(Parent parent);
        Parent? Update(int id, Parent parent); //
        bool Delete(int id);
    }
}