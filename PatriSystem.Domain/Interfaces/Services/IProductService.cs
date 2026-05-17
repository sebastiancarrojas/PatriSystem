using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<Response<object>> CreateAsync(Product product);
    }
}