using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IUnitOfMeasureService
    {
        Task<Response<UnitOfMeasure>> CreateAsync(UnitOfMeasure unitOfMeasure);
        Task<Response<List<UnitOfMeasure>>> GetAllAsync();
    }
}