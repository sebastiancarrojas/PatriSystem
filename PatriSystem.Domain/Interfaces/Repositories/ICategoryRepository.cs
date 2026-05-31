using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Pagination;

namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task CreateAsync(Category category);
        Task<bool> ExistsWithNameAsync(string name);
        Task<Category?> GetByNameAsync(string name);
        Task UpdateAsync(Category category);
        Task<Category?> GetByIdAsync(Guid id);
        Task<PaginationResponse<Category>> GetPaginatedAsync(CategoryPaginationRequest request);
    }
}