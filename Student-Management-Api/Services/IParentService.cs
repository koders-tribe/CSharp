using StudentManagementAPI.Models;
using StudentManagementAPI.Services.Dtos;

namespace StudentManagementAPI.Services
{
    public interface IParentService
    {
        List<Parent> GetAllParents();
        Parent? GetParentById(int id);
        Parent? GetParentWithStudents(int id);
        Parent CreateParent (CreateParentDto dto);
        Parent? UpdateParent(int id, UpdateParentDto dto);
        bool DeleteParent(int id);
    }
}