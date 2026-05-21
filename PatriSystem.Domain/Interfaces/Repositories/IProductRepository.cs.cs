using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task CreateAsync(Product product);
        Task<bool> ExistsWithBarcodeAsync(string barcode);
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
        Task DeactivateAsync(Guid id);
    }
}