using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task CreateAsync(Category category);
        Task<bool> ExistsWithNameAsync(string name);
        Task<Category?> GetByNameAsync(string name);
    }
}