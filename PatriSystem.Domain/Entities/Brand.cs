namespace PatriSystem.Domain.Entities
{
    public class Brand : AuditBase
    {
        public string BrandName { get; set; } = string.Empty;
        public string BrandDescription { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}