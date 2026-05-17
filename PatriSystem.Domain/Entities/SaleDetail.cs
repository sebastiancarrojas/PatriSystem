namespace PatriSystem.Domain.Entities
{
    public class SaleDetail : AuditBase
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

        // Foreign Keys
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }

        // Navigation Properties
        public Sale Sale { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}