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

namespace StudentManagementAPI.Controllers
{
    [ApiController]                          // ← This is an API controller
    [Route("api/[controller]")]              // ← Route: /api/students
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        
        // ← KEY: Service is INJECTED via constructor
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        
        // ────────────────────────────────────────
        // ENDPOINT 1: GET all students
        // ────────────────────────────────────────
        [HttpGet]                            // ← Responds to GET /api/students
        public IActionResult GetAllStudents()
        {
            try
            {
                // ← CLEAN: Just call the service
                var students = _studentService.GetAllStudents();
                
                // Return HTTP 200 OK with list
                return Ok(students);
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }
        
        // ────────────────────────────────────────
        // ENDPOINT 2: GET student by ID
        // ────────────────────────────────────────
        [HttpGet("{id}")]                    // ← Responds to GET /api/students/{id}
        public IActionResult GetStudentById(int id)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                
                if (student == null)
                    return NotFound(new { message = $"Student {id} not found" });
                
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }
        
        // ────────────────────────────────────────
        // ENDPOINT 3: POST create student
        // ────────────────────────────────────────
        [HttpPost]                           // ← Responds to POST /api/students
        public IActionResult CreateStudent([FromBody] CreateStudentDto dto)
        {
            // [FromBody] = read request body as CreateStudentDto
            
            try
            {
                var student = _studentService.CreateStudent(dto);
                
                // Return HTTP 201 Created with the new student
                return CreatedAtAction(nameof(GetStudentById), 
                                     new { id = student.Id }, 
                                     student);
            }
            catch (ArgumentException ex)
            {
                // Validation error = HTTP 400
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Server error = HTTP 500
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }
        
        // ────────────────────────────────────────
        // ENDPOINT 4: DELETE student
        // ────────────────────────────────────────
        [HttpDelete("{id}")]                 // ← Responds to DELETE /api/students/{id}
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                var result = _studentService.DeleteStudent(id);
                
                if (!result)
                    return NotFound(new { message = $"Student {id} not found" });
                
                return Ok(new { message = "Student deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }
    }
}