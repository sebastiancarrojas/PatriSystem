using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Pagination;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface ISaleService
    {
        Task<Response<Guid>> CreateAsync(Sale sale);
        Task<Response<List<Sale>>> GetAllAsync();
        Task<Response<Sale>> GetByIdAsync(Guid id);
        Task<Response<PaginationResponse<Sale>>> GetPaginatedAsync(SalePaginationRequest request);
    }
}