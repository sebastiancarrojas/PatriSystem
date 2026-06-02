using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.Domain.Interfaces.Repositories;

namespace PatriSystem.DataAccess.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly PatriSystemDbContext _context;

        public DashboardRepository(PatriSystemDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetSalesTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Sales.CountAsync(s => s.SaleDate.Date == today);
        }

        public async Task<decimal> GetTotalTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .SumAsync(s => s.TotalAmount);
        }

        public async Task<int> GetTotalProductsAsync()
            => await _context.Products.CountAsync();

        public async Task<int> GetActiveProductsAsync()
            => await _context.Products.CountAsync(p => p.Status == true);

        public async Task<int> GetTotalSalesAsync()
            => await _context.Sales.CountAsync();

        public async Task<decimal> GetTotalRevenueAsync()
            => await _context.Sales.SumAsync(s => s.TotalAmount);
    }
}