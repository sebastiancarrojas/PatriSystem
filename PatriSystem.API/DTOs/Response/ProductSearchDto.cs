namespace PatriSystem.API.DTOs.Response
{
    public class ProductSearchDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public decimal UnitPrice { get; set; }
        public int CurrentStock { get; set; }
    }
}