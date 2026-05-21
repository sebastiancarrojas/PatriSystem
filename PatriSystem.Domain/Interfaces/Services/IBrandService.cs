using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IBrandService
    {
        Task<Response<object>> CreateAsync(Brand brand);
        Task<Response<List<Brand>>> GetAllAsync();
    }
}