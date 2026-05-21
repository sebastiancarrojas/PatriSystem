namespace PatriSystem.API.DTOs.Response
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string? ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public string? UnitOfMeasure { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Category
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        // Brand
        public Guid BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
    }
}