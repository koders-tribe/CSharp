// File: Data/StudentRepository.cs

using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public class StudentRepository : IStudentRepository  // ← Implements the interface
    {
        // Temporary in-memory storage (Day 3 we'll replace with database)
        private static List<Student> students = new()
        {
            new Student { Id = 1, Name = "Balaji", Email = "balaji@example.com", Phone = "9876543210", CreatedAt = DateTime.Now },
            new Student { Id = 2, Name = "John", Email = "john@example.com", Phone = "9876543211", CreatedAt = DateTime.Now }
        };
        
        // ← MUST implement GetAll() (from interface)
        public List<Student> GetAll()
        {
            // Returns all students from in-memory list
            return students;
        }
        
        // ← MUST implement GetById() (from interface)
        public Student? GetById(int id)
        {
            // Find student by ID
            return students.FirstOrDefault(s => s.Id == id);
            // FirstOrDefault = returns student or null if not found
        }
        
        // ← MUST implement Add() (from interface)
        public Student Add(Student student)
        {
            // Generate ID (max ID + 1)
            student.Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
            
            // Set creation date
            student.CreatedAt = DateTime.Now;
            
            // Add to list
            students.Add(student);
            
            // Return the added student (with generated ID)
            return student;
        }
        
        // ← MUST implement Delete() (from interface)
        public bool Delete(int id)
        {
            var student = GetById(id);  // Find the student
            
            if (student != null)
            {
                students.Remove(student);  // Remove from list
                return true;               // Success
            }
            
            return false;  // Not found
        }
    }
}