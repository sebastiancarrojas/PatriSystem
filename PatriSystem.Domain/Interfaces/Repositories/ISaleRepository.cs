using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface ISaleRepository
    {
        Task<Guid> CreateAsync(Sale sale, List<Product> products);
        Task<List<Sale>> GetAllAsync();
        Task<Sale?> GetByIdAsync(Guid id);
    }
}