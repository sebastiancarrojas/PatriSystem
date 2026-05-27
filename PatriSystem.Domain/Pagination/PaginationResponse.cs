namespace PatriSystem.Domain.Pagination
{
    public class PaginationResponse<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int RecordsPerPage { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public string? Filter { get; set; }
        public List<T> Items { get; set; } = new();
    }
}