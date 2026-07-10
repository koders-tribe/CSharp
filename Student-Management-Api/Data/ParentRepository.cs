using StudentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementAPI.Data
{
    public class ParentRepository : IParentRepository
    {
        private readonly AppDbContext _context;

        public ParentRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all parents
        public List<Parent> GetAll()
        {
            return _context.Parents.ToList();
        }

        // Get parent by ID (without students)
        public Parent? GetById(int id)
        {
            return _context.Parents.FirstOrDefault(p => p.Id == id);
        }

        // Get parent by ID with all students (includes students)
        public Parent? GetByIdWithStudents(int id)
        {
            return _context.Parents
                .Include(p => p.Students)  // Load students
                .FirstOrDefault(p => p.Id == id);
        }

        // Add new parent
        public Parent Add(Parent parent)
        {
            parent.CreatedAt = DateTime.Now;
            _context.Parents.Add(parent);
            _context.SaveChanges();
            return parent;
        }

        // Update parent
        public Parent? Update(int id, Parent parent)
        {
            var existing = GetById(id);
            if (existing == null)
                return null;

            existing.Name = parent.Name ?? existing.Name;
            existing.Email = parent.Email ?? existing.Email;
            existing.Phone = parent.Phone ?? existing.Phone;
            existing.Occupation = parent.Occupation ?? existing.Occupation;
            existing.Relationship = parent.Relationship ?? existing.Relationship;
            existing.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return existing;
        }

        // Delete parent
        public bool Delete(int id)
        {
            var parent = GetById(id);
            if (parent == null)
                return false;

            _context.Parents.Remove(parent);
            _context.SaveChanges();
            return true;
        }
    }
}