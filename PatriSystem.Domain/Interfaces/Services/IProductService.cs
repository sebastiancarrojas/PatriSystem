using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<Response<object>> CreateAsync(Product product);
        Task<Response<object>> UpdateAsync(Guid id, Product product);
        Task<Response<object>> DeactivateAsync(Guid id);
        Task<Response<List<Product>>> GetAllAsync();
        Task<Response<Product>> GetByIdAsync(Guid id);
    }
}