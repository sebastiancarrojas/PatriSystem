namespace PatriSystem.Domain.Pagination
{
    public class SalePaginationRequest : PaginationRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}