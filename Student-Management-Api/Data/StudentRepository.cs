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

        // ═══════════════════════════════════════════════════════════════
        // DAY 5: NEW METHODS FOR PAGINATION, FILTERING & SEARCH
        // ═══════════════════════════════════════════════════════════════

        public async Task<List<Student>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? grade = null,
            int? parentId = null,
            string sortBy = "name",
            bool descending = false)
        {
            // WHY: Build query step by step
            var query = _context.Students.AsQueryable();

            // STEP 1: Apply search filter
            // WHY: Search in multiple fields (name, email, phone)
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(s =>
                    (s.Name ?? "").ToLower().Contains(searchLower) ||
                    (s.Email ?? "").ToLower().Contains(searchLower) ||
                    (s.Phone ?? "").Contains(search)
                );
            }

            // STEP 2: Apply grade filter
            // WHY: Simple equality check
            if (grade.HasValue)
            {
                query = query.Where(s => s.Grade == grade.Value);
            }

            // STEP 3: Apply parent filter
            // WHY: Filter by relationship
            if (parentId.HasValue)
            {
                query = query.Where(s => s.ParentId == parentId.Value);
            }

            // STEP 4: Apply sorting
            // WHY: Switch expression for clean routing
            query = (sortBy.ToLower(), descending) switch
            {
                ("name", false) => query.OrderBy(s => s.Name),
                ("name", true) => query.OrderByDescending(s => s.Name),
                ("grade", false) => query.OrderBy(s => s.Grade),
                ("grade", true) => query.OrderByDescending(s => s.Grade),
                ("date", false) => query.OrderBy(s => s.DateOfBirth),
                ("date", true) => query.OrderByDescending(s => s.DateOfBirth),
                _ => query.OrderBy(s => s.Name)  // Default
            };

            // STEP 5: Apply pagination
            // WHY: Skip first N, take next N
            var result = await query
                .Skip((page - 1) * pageSize)    // Skip formula: (page-1) * size
                .Take(pageSize)                 // Take exact page size
                .Include(s => s.Parent)         // Load relationships (single JOIN)
                .AsNoTracking()                 // Read-only (performance)
                .ToListAsync();                 // Execute async

            return result;
        }

        public async Task<int> GetCountAsync(
            string? search = null,
            int? grade = null,
            int? parentId = null)
        {
            // WHY: Same filters as GetPaginatedAsync, but just count
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(s =>
                    (s.Name ?? "").ToLower().Contains(searchLower) ||
                    (s.Email ?? "").ToLower().Contains(searchLower) ||
                    (s.Phone ?? "").Contains(search)
                );
            }

            if (grade.HasValue)
            {
                query = query.Where(s => s.Grade == grade.Value);
            }

            if (parentId.HasValue)
            {
                query = query.Where(s => s.ParentId == parentId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<List<Student>> SearchAsync(string search)
        {
            // WHY: Simple search without pagination
            if (string.IsNullOrEmpty(search))
                return await _context.Students.ToListAsync();

            var searchLower = search.ToLower();
            return await _context.Students
                .Where(s =>
                    (s.Name ?? "").ToLower().Contains(searchLower) ||
                    (s.Email ?? "").ToLower().Contains(searchLower)
                )
                .OrderBy(s => s.Name)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}