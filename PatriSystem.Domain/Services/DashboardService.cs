using PatriSystem.Domain.Common;
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

        public async Task<Response<object>> GetDashboardAsync()
        {
            try
            {
                var salesToday = await _dashboardRepository.GetSalesTodayAsync();
                var totalToday = await _dashboardRepository.GetTotalTodayAsync();
                var totalProducts = await _dashboardRepository.GetTotalProductsAsync();
                var activeProducts = await _dashboardRepository.GetActiveProductsAsync();
                var totalSales = await _dashboardRepository.GetTotalSalesAsync();
                var totalRevenue = await _dashboardRepository.GetTotalRevenueAsync();

                var dashboard = new
                {
                    salesToday,
                    totalToday,
                    totalProducts,
                    activeProducts,
                    inactiveProducts = totalProducts - activeProducts,
                    totalSales,
                    totalRevenue
                };

                return Response<object>.Success(dashboard);
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex, "Error al obtener el dashboard");
            }
        }
    }
}