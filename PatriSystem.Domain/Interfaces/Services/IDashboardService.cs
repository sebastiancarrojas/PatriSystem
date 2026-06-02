using PatriSystem.Domain.Common;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<Response<object>> GetDashboardAsync();
    }
}