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
        Task<IEnumerable<object>> GetLast7DaysSalesAsync();
        Task<IEnumerable<object>> GetTopProductsLastMonthAsync();
    }
}