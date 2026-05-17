namespace PatriSystem.Domain.Entities
{
    public class Product : AuditBase
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int CurrentStock { get; set; }
        public int StockMin { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;

        // Foreign Keys
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }

        // Navigation Properties
        public Category Category { get; set; } = null!;
        public Brand Brand { get; set; } = null!;
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}