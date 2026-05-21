using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Response<object>> CreateAsync(Category category);
        Task<Response<List<Category>>> GetAllAsync();
    }
}