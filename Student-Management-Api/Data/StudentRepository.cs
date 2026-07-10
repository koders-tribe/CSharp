using StudentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementAPI.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all students
        public List<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        // Get student by ID (without parent)
        public Student? GetById(int id)
        {
            return _context.Students.FirstOrDefault(s => s.Id == id);
        }

        // Get student by ID with parent (eager loading)
        public Student? GetByIdWithParent(int id)
        {
            return _context.Students
                .Include(s => s.Parent)
                .FirstOrDefault(s => s.Id == id);
        }

        // Add new student
        public Student Add(Student student)
        {
            student.CreatedAt = DateTime.Now;
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        // Update student
        public Student? Update(int id, Student student)
        {
            var existing = GetById(id);
            if (existing == null)
                return null;

            existing.Name = student.Name ?? existing.Name;
            existing.Email = student.Email ?? existing.Email;
            existing.Phone = student.Phone ?? existing.Phone;
            existing.RollNumber = student.RollNumber ?? existing.RollNumber;
            existing.Grade = student.Grade > 0 ? student.Grade : existing.Grade;
            existing.DateOfBirth = student.DateOfBirth != default ? student.DateOfBirth : existing.DateOfBirth;
            existing.ParentId = student.ParentId ?? existing.ParentId;
            existing.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return existing;
        }

        // Delete student
        public bool Delete(int id)
        {
            var student = GetById(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            _context.SaveChanges();
            return true;
        }
    }
}
