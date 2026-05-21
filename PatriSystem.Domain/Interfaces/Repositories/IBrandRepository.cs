using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllAsync();
        Task CreateAsync(Brand brand);
        Task<bool> ExistsWithNameAsync(string name);
    }
}