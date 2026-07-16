namespace StudentManagementAPI.Services.Dtos
{
    /// <summary>
    /// Generic pagination response wrapper
    /// Why: Can reuse for ANY entity (Students, Parents, Teachers)
    /// </summary>
    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    /// <summary>
    /// Student filter/search/pagination parameters
    /// Why: Encapsulates all query parameters in one object
    /// </summary>
    public class StudentFilterDto
    {
        // Search criteria
        public string Search { get; set; } = "";

        // Filter criteria
        public int? Grade { get; set; }
        public int? ParentId { get; set; }

        // Sorting
        public string SortBy { get; set; } = "name";
        public bool Descending { get; set; } = false;

        // Pagination
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}