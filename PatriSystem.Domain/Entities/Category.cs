namespace PatriSystem.Domain.Entities
{
    public class Category : AuditBase
    {
        public string CategoryName { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}