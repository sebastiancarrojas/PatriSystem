using PatriSystem.Domain.Common;
using PatriSystem.Domain.DTOs.Dashboard;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Interfaces.Services;

namespace PatriSystem.Domain.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<Response<DashboardResponse>> GetDashboardAsync()
        {
            try
            {
                var dashboard = new DashboardResponse
                {
                    SalesTodayCount = await _dashboardRepository.GetSalesTodayCountAsync(),
                    SalesTodayAmount = await _dashboardRepository.GetSalesTodayAmountAsync(),
                    SalesWeekCount = await _dashboardRepository.GetSalesWeekCountAsync(),
                    SalesWeekAmount = await _dashboardRepository.GetSalesWeekAmountAsync(),
                    SalesMonthCount = await _dashboardRepository.GetSalesMonthCountAsync(),
                    SalesMonthAmount = await _dashboardRepository.GetSalesMonthAmountAsync(),
                    TotalProducts = await _dashboardRepository.GetTotalProductsAsync(),
                    Last7DaysSales = await _dashboardRepository.GetLast7DaysSalesAsync(),
                    TopProductsLastMonth = await _dashboardRepository.GetTopProductsLastMonthAsync()
                };

                return Response<DashboardResponse>.Success(dashboard);
            }
            catch (Exception ex)
            {
                return Response<DashboardResponse>.Failure(ex, "Error al obtener el dashboard");
            }
        }
    }
}