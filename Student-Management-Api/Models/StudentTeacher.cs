
namespace StudentManagementAPI.Models
{
    public class StudentTeacher
    {
        // ===== Foreign Keys (Composite Key) =====
        public int StudentId { get; set; }      // FK to Students table
        public int TeacherId { get; set; }      // FK to Teachers table
        
        // ===== Navigation Properties =====
        public Student? Student { get; set; }    // Access student
        public Teacher? Teacher { get; set; }    // Access teacher
        
        // ===== Additional Metadata (Optional) =====
        public DateTime AssignedDate { get; set; }  // When assigned
        public string? Subject { get; set; }         // Subject taught
    }
}