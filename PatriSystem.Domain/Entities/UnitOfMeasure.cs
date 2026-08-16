namespace PatriSystem.Domain.Entities
{
    public class UnitOfMeasure : AuditBase
    {
        public string Name { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}