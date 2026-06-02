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
                var salesTodayCount = await _dashboardRepository.GetSalesTodayCountAsync();
                var salesTodayAmount = await _dashboardRepository.GetSalesTodayAmountAsync();
                var salesWeekCount = await _dashboardRepository.GetSalesWeekCountAsync();
                var salesWeekAmount = await _dashboardRepository.GetSalesWeekAmountAsync();
                var salesMonthCount = await _dashboardRepository.GetSalesMonthCountAsync();
                var salesMonthAmount = await _dashboardRepository.GetSalesMonthAmountAsync();
                var totalProducts = await _dashboardRepository.GetTotalProductsAsync();
                var last7DaysSales = await _dashboardRepository.GetLast7DaysSalesAsync();
                var topProducts = await _dashboardRepository.GetTopProductsLastMonthAsync();

                var dashboard = new
                {
                    salesTodayCount,
                    salesTodayAmount,
                    salesWeekCount,
                    salesWeekAmount,
                    salesMonthCount,
                    salesMonthAmount,
                    totalProducts,
                    last7DaysSales,
                    topProducts
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