using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface ISaleService
    {
        Task<Response<Guid>> CreateAsync(Sale sale);
        Task<Response<List<Sale>>> GetAllAsync();
        Task<Response<Sale>> GetByIdAsync(Guid id);
    }
}