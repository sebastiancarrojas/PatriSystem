using PatriSystem.Domain.Common;
using PatriSystem.Domain.DTOs.Dashboard;

namespace PatriSystem.Domain.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<Response<DashboardResponse>> GetDashboardAsync();
    }
}