using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Pagination;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IBrandService
    {
        Task<Response<Brand>> CreateAsync(Brand brand);
        Task<Response<List<Brand>>> GetAllAsync();
        Task<Response<object>> UpdateAsync(Guid id, Brand brand);
        Task<Response<PaginationResponse<Brand>>> GetPaginatedAsync(BrandPaginationRequest request);
    }
}