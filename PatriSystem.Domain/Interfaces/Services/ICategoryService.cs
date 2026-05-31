using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Pagination;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Response<Category>> CreateAsync(Category category);
        Task<Response<List<Category>>> GetAllAsync();
        Task<Response<object>> UpdateAsync(Guid id, Category category);
        Task<Response<PaginationResponse<Category>>> GetPaginatedAsync(CategoryPaginationRequest request);
    }
}