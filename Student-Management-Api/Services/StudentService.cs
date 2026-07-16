using StudentManagementAPI.Data;
using StudentManagementAPI.Models;
using StudentManagementAPI.Services.Dtos;

namespace StudentManagementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        
        // ← Repository injected via constructor
        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }
        
        public List<Student> GetAllStudents()
        {
            return _repository.GetAll();
        }
        
        public Student? GetStudentById(int id)
        {
            return _repository.GetById(id);
        }

        public Student? GetStudentWithParent(int id)
        {
            return _repository.GetByIdWithParent(id);
        }

        public StudentWithParentDto? GetStudentWithParentDto(int id)
        {
            var student = _repository.GetByIdWithParent(id);
            if (student == null)
                return null;

            return new StudentWithParentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                RollNumber = student.RollNumber,
                Grade = student.Grade,
                DateOfBirth = student.DateOfBirth,
                ParentId = student.ParentId,
                CreatedAt = student.CreatedAt,
                UpdatedAt = student.UpdatedAt,
                Parent = student.Parent == null ? null : new ParentDto
                {
                    Id = student.Parent.Id,
                    Name = student.Parent.Name,
                    Email = student.Parent.Email,
                    Phone = student.Parent.Phone,
                    Occupation = student.Parent.Occupation,
                    Relationship = student.Parent.Relationship,
                    CreatedAt = student.Parent.CreatedAt,
                    UpdatedAt = student.Parent.UpdatedAt
                }
            };
        }

        public Student CreateStudent(CreateStudentDto dto)
        {
            // Business logic: validation
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required");
            
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required");
            
            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                RollNumber = dto.RollNumber,
                Grade = dto.Grade,
                DateOfBirth = dto.DateOfBirth,
                ParentId = dto.ParentId
            };

            return _repository.Add(student);
        }

        public Student? UpdateStudent(int id, UpdateStudentDto dto)
        {
            var student = GetStudentById(id);
            if (student == null)
                return null;

            var updatedStudent = new Student
            {
                Id = student.Id,
                Name = dto.Name ?? student.Name,
                Email = dto.Email ?? student.Email,
                Phone = dto.Phone ?? student.Phone,
                RollNumber = dto.RollNumber ?? student.RollNumber,
                Grade = dto.Grade > 0 ? dto.Grade : student.Grade,
                DateOfBirth = dto.DateOfBirth != default ? dto.DateOfBirth : student.DateOfBirth,
                ParentId = dto.ParentId ?? student.ParentId
            };

            return _repository.Update(id, updatedStudent);
        }

        public bool DeleteStudent(int id)
        {
            return _repository.Delete(id);
        }

        // ═══════════════════════════════════════════════════════════════
        // DAY 5: PAGINATION, FILTERING & SEARCH METHODS
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Get paginated, filtered, and sorted students with complete response
        /// </summary>
        public async Task<PaginatedResponse<StudentDto>> GetStudentsPaginatedAsync(
            StudentFilterDto filter)
        {
            // Validate input parameters
            if (filter.Page < 1) 
                filter.Page = 1;
            if (filter.PageSize < 1 || filter.PageSize > 100) 
                filter.PageSize = 10;

            // Get paginated data from repository
            var students = await _repository.GetPaginatedAsync(
                page: filter.Page,
                pageSize: filter.PageSize,
                search: filter.Search,
                grade: filter.Grade,
                parentId: filter.ParentId,
                sortBy: filter.SortBy,
                descending: filter.Descending
            );

            // Get total count for pagination info
            var totalRecords = await _repository.GetCountAsync(
                search: filter.Search,
                grade: filter.Grade,
                parentId: filter.ParentId
            );

            // Convert models to DTOs (prevent circular references)
            var dtos = students.Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone,
                RollNumber = s.RollNumber,
                Grade = s.Grade,
                DateOfBirth = s.DateOfBirth,
                ParentId = s.ParentId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Parent = s.Parent == null ? null : new ParentDto
                {
                    Id = s.Parent.Id,
                    Name = s.Parent.Name,
                    Email = s.Parent.Email,
                    Phone = s.Parent.Phone,
                    Occupation = s.Parent.Occupation,
                    Relationship = s.Parent.Relationship
                }
            }).ToList();

            // Calculate pagination info
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);

            return new PaginatedResponse<StudentDto>
            {
                Data = dtos,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                HasPreviousPage = filter.Page > 1,
                HasNextPage = filter.Page < totalPages
            };
        }

        /// <summary>
        /// Simple search students by name or email
        /// Returns all matching students without pagination
        /// </summary>
        public async Task<List<StudentDto>> SearchStudentsAsync(string search)
        {
            if (string.IsNullOrEmpty(search))
                return new List<StudentDto>();

            var students = await _repository.SearchAsync(search);

            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone,
                Grade = s.Grade
            }).ToList();
        }

        /// <summary>
        /// Get all students of a specific grade
        /// </summary>
        public async Task<List<StudentDto>> GetStudentsByGradeAsync(int grade)
        {
            var filter = new StudentFilterDto
            {
                Grade = grade,
                Page = 1,
                PageSize = 100  // Get all
            };

            var result = await GetStudentsPaginatedAsync(filter);
            return result.Data;
        }
    }
}