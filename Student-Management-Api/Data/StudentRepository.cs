using StudentManagementAPI.Models;
using StudentManagementAPI.Data;

namespace StudentManagementAPI.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;  // ← Changed from List!
        
        public StudentRepository(AppDbContext context)  // ← New constructor!
        {
            _context = context;
        }
        
        public List<Student> GetAll()
        {
            return _context.Students.ToList();  // ← Changed from students.ToList()!
        }
        
        public Student? GetById(int id)
        {
            return _context.Students.FirstOrDefault(s => s.Id == id);  // ← Changed!
        }
        
        public Student Add(Student student)
        {
            _context.Students.Add(student);      // ← Changed!
            _context.SaveChanges();              // ← New: commit to database!
            return student;
        }
        
        public bool Delete(int id)
        {
            var student = GetById(id);
            if (student == null)
                return false;
            
            _context.Students.Remove(student);   // ← Changed!
            _context.SaveChanges();              // ← New: commit to database!
            return true;
        }
    }
}