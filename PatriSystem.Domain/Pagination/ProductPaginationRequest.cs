namespace PatriSystem.Domain.Pagination
{
    public class ProductPaginationRequest : PaginationRequest
    {
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public bool? Status { get; set; }
    }
}