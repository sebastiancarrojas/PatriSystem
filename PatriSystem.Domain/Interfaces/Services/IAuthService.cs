using PatriSystem.Domain.Common;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Response<string>> LoginAsync(string email, string password);
    }
}