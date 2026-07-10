// File: Controllers/StudentsController.cs
// Important Points:

// Controller ONLY handles HTTP
// Controller calls service for all logic
// Controller has error handling (try-catch)
// Controller returns appropriate HTTP status codes (200, 201, 400, 404, 500)
// Controller uses DTOs for input/output

using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Data;
using StudentManagementAPI.Services;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IParentRepository _parentRepository;

        public StudentsController(IStudentService studentService, IParentRepository parentRepository)
        {
            _studentService = studentService;
            _parentRepository = parentRepository;
        }

        // GET: api/students
        // Get all students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            try
            {
                var students = _studentService.GetAllStudents();
                return Ok(new { success = true, data = students, count = students.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // GET: api/students/{id}
        // Get student by ID
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                if (student == null)
                    return NotFound(new { success = false, message = $"Student with ID {id} not found" });

                return Ok(new { success = true, data = student });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // GET: api/students/{id}/with-parent
        // Get student with parent (using DTO to avoid circular reference)
        [HttpGet("{id}/with-parent")]
        public IActionResult GetStudentWithParent(int id)
        {
            try
            {
                var student = _studentService.GetStudentWithParentDto(id);
                if (student == null)
                    return NotFound(new { success = false, message = $"Student with ID {id} not found" });

                return Ok(new { success = true, data = student });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }
        // POST: api/students
        // Create new student
        [HttpPost]
        public IActionResult CreateStudent([FromBody] CreateStudentDto dto)
        {
            try
            {
                var student = _studentService.CreateStudent(dto);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id },
                    new { success = true, data = student, message = "Student created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // PUT: api/students/{id}
        // Update student
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] UpdateStudentDto dto)
        {
            try
            {
                var student = _studentService.UpdateStudent(id, dto);
                if (student == null)
                    return NotFound(new { success = false, message = $"Student with ID {id} not found" });

                return Ok(new { success = true, data = student, message = "Student updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // DELETE: api/students/{id}
        // Delete student
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                var result = _studentService.DeleteStudent(id);
                if (!result)
                    return NotFound(new { success = false, message = $"Student with ID {id} not found" });

                return Ok(new { success = true, message = "Student deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // PUT: api/students/{id}/assign-parent/{parentId}
        // Assign a parent to a student (relationship operation)
        [HttpPut("{id}/assign-parent/{parentId}")]
        public IActionResult AssignParent(int id, int parentId)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                if (student == null)
                    return NotFound(new { success = false, message = $"Student with ID {id} not found" });

                var parent = _parentRepository.GetById(parentId);
                if (parent == null)
                    return NotFound(new { success = false, message = $"Parent with ID {parentId} not found" });

                // Update student's parent
                var updateDto = new UpdateStudentDto
                {
                    ParentId = parentId
                };

                var updatedStudent = _studentService.UpdateStudent(id, updateDto);

                // Convert to DTO to avoid circular reference
                var resultDto = _studentService.GetStudentWithParentDto(id);

                return Ok(new { success = true, data = resultDto, message = "Parent assigned successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }
        // PUT: api/students/{id}/remove-parent
        // Remove parent from student
        [HttpPut("{id}/remove-parent")]
        public IActionResult RemoveParent(int id)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                if (student == null)
                    return NotFound(new { success = false, message = $"Student with ID {id} not found" });

                // Update student's parent to null
                var updateDto = new UpdateStudentDto
                {
                    ParentId = null
                };

                var updatedStudent = _studentService.UpdateStudent(id, updateDto);

                return Ok(new { success = true, data = updatedStudent, message = "Parent removed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }
    }
}
