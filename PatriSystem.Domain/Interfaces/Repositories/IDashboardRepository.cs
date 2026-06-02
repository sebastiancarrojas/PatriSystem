using PatriSystem.Domain.DTOs.Dashboard;

namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetSalesTodayCountAsync();
        Task<decimal> GetSalesTodayAmountAsync();
        Task<int> GetSalesWeekCountAsync();
        Task<decimal> GetSalesWeekAmountAsync();
        Task<int> GetSalesMonthCountAsync();
        Task<decimal> GetSalesMonthAmountAsync();
        Task<int> GetTotalProductsAsync();
        Task<IEnumerable<DailySalesResponseDto>> GetLast7DaysSalesAsync();
        Task<IEnumerable<TopProductResponseDto>> GetTopProductsLastMonthAsync();
    }
}