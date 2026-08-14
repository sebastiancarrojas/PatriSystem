using PatriSystem.Domain.Common;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Response<string>> LoginAsync(string email, string password);
        Task<Response<string>> ForgotPasswordAsync(string email);
        Task<Response<string>> ResetPasswordAsync(string email, string token, string newPassword);
    }
}