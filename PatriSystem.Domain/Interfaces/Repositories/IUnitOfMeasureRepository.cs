using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface IUnitOfMeasureRepository
    {
        Task<List<UnitOfMeasure>> GetAllAsync();
        Task CreateAsync(UnitOfMeasure unitOfMeasure);
        Task<bool> ExistsWithNameAsync(string name);
        Task<UnitOfMeasure?> GetByNameAsync(string name);
        Task<UnitOfMeasure?> GetByIdAsync(Guid id);
    }
}