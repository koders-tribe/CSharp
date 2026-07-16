using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public interface IStudentRepository
    {
        // Existing synchronous methods
        List<Student> GetAll();
        Student? GetById(int id);
        Student? GetByIdWithParent(int id);
        Student Add(Student student);
        Student? Update(int id, Student student);
        bool Delete(int id);

        // NEW: Day 5 Asynchronous methods for pagination, filtering & search
        
        /// <summary>
        /// Get paginated, filtered, sorted students
        /// </summary>
        Task<List<Student>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? grade = null,
            int? parentId = null,
            string sortBy = "name",
            bool descending = false);

        /// <summary>
        /// Get total count of students matching filters
        /// </summary>
        Task<int> GetCountAsync(
            string? search = null,
            int? grade = null,
            int? parentId = null);

        /// <summary>
        /// Simple search by name/email
        /// </summary>
        Task<List<Student>> SearchAsync(string search);
    }
}