using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Pagination;

namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllAsync();
        Task CreateAsync(Brand brand);
        Task<bool> ExistsWithNameAsync(string name);
        Task<Brand?> GetByNameAsync(string name);
        Task UpdateAsync (Brand brand);
        Task<Brand?> GetByIdAsync(Guid id);
        Task<PaginationResponse<Brand>> GetPaginatedAsync(BrandPaginationRequest request);
    }
}