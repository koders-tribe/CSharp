using StudentManagementAPI.Data;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Services
{
    // DTO for creating parent
    public class CreateParentDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Occupation { get; set; }
        public string? Relationship { get; set; }
    }

    // DTO for updating parent
    public class UpdateParentDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Occupation { get; set; }
        public string? Relationship { get; set; }
    }

    public class ParentService : IParentService
    {
        private readonly IParentRepository _repository;

        public ParentService(IParentRepository repository)
        {
            _repository = repository;
        }

        // Get all parents
        public List<Parent> GetAllParents()
        {
            return _repository.GetAll();
        }

        // Get parent by ID
        public Parent? GetParentById(int id)
        {
            return _repository.GetById(id);
        }

        // Get parent with their students
        public Parent? GetParentWithStudents(int id)
        {
            return _repository.GetByIdWithStudents(id);
        }

        // Create parent
        public Parent CreateParent(CreateParentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Parent name is required");

            var parent = new Parent
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Occupation = dto.Occupation,
                Relationship = dto.Relationship
            };

            return _repository.Add(parent);
        }

        // Update parent
        public Parent? UpdateParent(int id, UpdateParentDto dto)
        {
            var parent = GetParentById(id);
            if (parent == null)
                return null;

            var updatedParent = new Parent
            {
                Id = parent.Id,
                Name = dto.Name ?? parent.Name,
                Email = dto.Email ?? parent.Email,
                Phone = dto.Phone ?? parent.Phone,
                Occupation = dto.Occupation ?? parent.Occupation,
                Relationship = dto.Relationship ?? parent.Relationship
            };

            return _repository.Update(id, updatedParent);
        }

        // Delete parent
        public bool DeleteParent(int id)
        {
            return _repository.Delete(id);
        }
    }
}