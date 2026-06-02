namespace PatriSystem.Domain.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetSalesTodayAsync();
        Task<decimal> GetTotalTodayAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetActiveProductsAsync();
        Task<int> GetTotalSalesAsync();
        Task<decimal> GetTotalRevenueAsync();
    }
}