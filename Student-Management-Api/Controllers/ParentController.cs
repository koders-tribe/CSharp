using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Services;
using StudentManagementAPI.Services.Dtos;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentsController : ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentsController(IParentService parentService)
        {
            _parentService = parentService;
        }

        // GET: api/parents
        // Get all parents
        [HttpGet]
        public IActionResult GetAllParents()
        {
            try
            {
                var parents = _parentService.GetAllParents();
                return Ok(new { success = true, data = parents, count = parents.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // GET: api/parents/{id}
        // Get parent by ID
        [HttpGet("{id}")]
        public IActionResult GetParentById(int id)
        {
            try
            {
                var parent = _parentService.GetParentById(id);
                if (parent == null)
                    return NotFound(new { success = false, message = $"Parent with ID {id} not found" });

                return Ok(new { success = true, data = parent });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // GET: api/parents/{id}/students
        // Get parent with all their students (relationship test!)
        [HttpGet("{id}/students")]
        public IActionResult GetParentWithStudents(int id)
        {
            try
            {
                var parent = _parentService.GetParentWithStudents(id);
                if (parent == null)
                    return NotFound(new { success = false, message = $"Parent with ID {id} not found" });

                return Ok(new { success = true, data = parent, studentCount = parent.Students.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // POST: api/parents
        // Create new parent
        [HttpPost]
        public IActionResult CreateParent([FromBody] CreateParentDto dto)
        {
            try
            {
                var parent = _parentService.CreateParent(dto);
                return CreatedAtAction(nameof(GetParentById), new { id = parent.Id }, 
                    new { success = true, data = parent, message = "Parent created successfully" });
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

        // PUT: api/parents/{id}
        // Update parent
        [HttpPut("{id}")]
        public IActionResult UpdateParent(int id, [FromBody] UpdateParentDto dto)
        {
            try
            {
                var parent = _parentService.UpdateParent(id, dto);
                if (parent == null)
                    return NotFound(new { success = false, message = $"Parent with ID {id} not found" });

                return Ok(new { success = true, data = parent, message = "Parent updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }

        // DELETE: api/parents/{id}
        // Delete parent
        [HttpDelete("{id}")]
        public IActionResult DeleteParent(int id)
        {
            try
            {
                var result = _parentService.DeleteParent(id);
                if (!result)
                    return NotFound(new { success = false, message = $"Parent with ID {id} not found" });

                return Ok(new { success = true, message = "Parent deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error", error = ex.Message });
            }
        }
    }
}
