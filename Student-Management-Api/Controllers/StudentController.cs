// File: Controllers/StudentsController.cs
// Important Points:

// Controller ONLY handles HTTP
// Controller calls service for all logic
// Controller has error handling (try-catch)
// Controller returns appropriate HTTP status codes (200, 201, 400, 404, 500)
// Controller uses DTOs for input/output


using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Models;
using StudentManagementAPI.Services;
using StudentManagementAPI.Services.Dtos;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        
        // ← Service injected via constructor (Dependency Injection!)
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        
        /// <summary>
        /// Get all students
        /// </summary>
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            try
            {
                var students = _studentService.GetAllStudents();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching students", error = ex.Message });
            }
        }
        
        /// <summary>
        /// Get student by ID
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                if (student == null)
                    return NotFound(new { message = $"Student with ID {id} not found" });
                
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching student", error = ex.Message });
            }
        }
        
        /// <summary>
        /// Create new student
        /// </summary>
        [HttpPost]
        public IActionResult CreateStudent([FromBody] CreateStudentDto dto)
        {
            try
            {
                var student = _studentService.CreateStudent(dto);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating student", error = ex.Message });
            }
        }
        
        /// <summary>
        /// Update student
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] UpdateStudentDto dto)
        {
            try
            {
                var student = _studentService.UpdateStudent(id, dto);
                if (student == null)
                    return NotFound(new { message = $"Student with ID {id} not found" });

                return Ok(new { success = true, data = student, message = "Student updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating student", error = ex.Message });
            }
        }

        /// <summary>
        /// Get student with parent relationship
        /// </summary>
        [HttpGet("{id}/with-parent")]
        public IActionResult GetStudentWithParent(int id)
        {
            try
            {
                var student = _studentService.GetStudentWithParent(id);
                if (student == null)
                    return NotFound(new { message = $"Student with ID {id} not found" });

                return Ok(new { success = true, data = student });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching student", error = ex.Message });
            }
        }

        /// <summary>
        /// Get student with parent as DTO (avoids circular reference)
        /// </summary>
        [HttpGet("{id}/with-parent-dto")]
        public IActionResult GetStudentWithParentDto(int id)
        {
            try
            {
                var student = _studentService.GetStudentWithParentDto(id);
                if (student == null)
                    return NotFound(new { message = $"Student with ID {id} not found" });

                return Ok(new { success = true, data = student });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching student", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete student
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                var result = _studentService.DeleteStudent(id);
                if (!result)
                    return NotFound(new { message = $"Student with ID {id} not found" });

                return Ok(new { message = "Student deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting student", error = ex.Message });
            }
        }

        // ═══════════════════════════════════════════════════════════════════════════════
        // DAY 5: PAGINATION, FILTERING & SEARCH ENDPOINTS
        // ═══════════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Get students with pagination, filtering, and search
        /// 
        /// Query Parameters:
        ///   - search: Search in name/email/phone (optional)
        ///   - grade: Filter by grade (optional)
        ///   - parentId: Filter by parent ID (optional)
        ///   - page: Page number (default: 1)
        ///   - pageSize: Items per page (default: 10, max: 100)
        ///   - sortBy: Sort by (name, grade, date) (default: name)
        ///   - descending: Sort descending (default: false)
        /// 
        /// Examples:
        ///   GET /api/students/search?page=1&pageSize=10
        ///   GET /api/students/search?search=balaji
        ///   GET /api/students/search?grade=10&page=1&pageSize=5
        ///   GET /api/students/search?search=bal&grade=10&sortBy=grade&descending=true
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> GetStudentsFiltered(
            [FromQuery] string search = "",
            [FromQuery] int? grade = null,
            [FromQuery] int? parentId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "name",
            [FromQuery] bool descending = false)
        {
            try
            {
                // Create filter object from query parameters
                var filter = new StudentFilterDto
                {
                    Search = search,
                    Grade = grade,
                    ParentId = parentId,
                    Page = page,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    Descending = descending
                };

                // Call service to get paginated data
                var result = await _studentService.GetStudentsPaginatedAsync(filter);

                // Return successful response with pagination metadata
                return Ok(new
                {
                    success = true,
                    message = "Students retrieved successfully",
                    data = result.Data,
                    pagination = new
                    {
                        totalRecords = result.TotalRecords,
                        totalPages = result.TotalPages,
                        currentPage = result.CurrentPage,
                        pageSize = result.PageSize,
                        hasPreviousPage = result.HasPreviousPage,
                        hasNextPage = result.HasNextPage
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    success = false, 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Quick search students by name or email (without pagination)
        /// 
        /// Example:
        ///   GET /api/students/quick-search?search=balaji
        /// </summary>
        [HttpGet("quick-search")]
        public async Task<IActionResult> QuickSearchStudents(
            [FromQuery] string search)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    return BadRequest(new 
                    { 
                        success = false, 
                        error = "Search term required" 
                    });

                var results = await _studentService.SearchStudentsAsync(search);

                return Ok(new
                {
                    success = true,
                    count = results.Count,
                    data = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    success = false, 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Get students by grade
        /// 
        /// Example:
        ///   GET /api/students/by-grade/10
        /// </summary>
        [HttpGet("by-grade/{grade}")]
        public async Task<IActionResult> GetStudentsByGrade(int grade)
        {
            try
            {
                if (grade < 1 || grade > 12)
                    return BadRequest(new 
                    { 
                        success = false, 
                        error = "Grade must be between 1 and 12" 
                    });

                var students = await _studentService.GetStudentsByGradeAsync(grade);

                return Ok(new
                {
                    success = true,
                    grade = grade,
                    count = students.Count,
                    data = students
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    success = false, 
                    error = ex.Message 
                });
            }
        }
    }
}