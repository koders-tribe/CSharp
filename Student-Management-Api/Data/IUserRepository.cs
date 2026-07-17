using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);
        Task AddAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}