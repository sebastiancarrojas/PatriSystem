namespace PatriSystem.Domain.Entities
{
    public class Sale : AuditBase
    {
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string SaleType { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}