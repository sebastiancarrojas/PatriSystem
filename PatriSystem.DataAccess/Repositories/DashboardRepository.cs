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

        public async Task<int> GetSalesTodayCountAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Sales.CountAsync(s => s.SaleDate.Date == today);
        }

        public async Task<decimal> GetSalesTodayAmountAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;
        }

        public async Task<int> GetSalesWeekCountAsync()
        {
            var weekStart = DateTime.UtcNow.Date.AddDays(-6);
            return await _context.Sales.CountAsync(s => s.SaleDate.Date >= weekStart);
        }

        public async Task<decimal> GetSalesWeekAmountAsync()
        {
            var weekStart = DateTime.UtcNow.Date.AddDays(-6);
            return await _context.Sales
                .Where(s => s.SaleDate.Date >= weekStart)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;
        }

        public async Task<int> GetSalesMonthCountAsync()
        {
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _context.Sales.CountAsync(s => s.SaleDate.Date >= monthStart);
        }

        public async Task<decimal> GetSalesMonthAmountAsync()
        {
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _context.Sales
                .Where(s => s.SaleDate.Date >= monthStart)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;
        }

        public async Task<int> GetTotalProductsAsync()
            => await _context.Products.CountAsync();

        public async Task<IEnumerable<object>> GetLast7DaysSalesAsync()
        {
            var weekStart = DateTime.UtcNow.Date.AddDays(-6);
            return await _context.Sales
                .Where(s => s.SaleDate.Date >= weekStart)
                .GroupBy(s => s.SaleDate.Date)
                .Select(g => new
                {
                    date = g.Key,
                    amount = g.Sum(s => s.TotalAmount)
                })
                .OrderBy(x => x.date)
                .Cast<object>()
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetTopProductsLastMonthAsync()
        {
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _context.SaleDetails
                .Where(sd => sd.Sale.SaleDate.Date >= monthStart && sd.ProductName != null)
                .GroupBy(sd => sd.ProductName)
                .Select(g => new
                {
                    productName = g.Key,
                    quantity = g.Sum(sd => sd.Quantity),
                    revenue = g.Sum(sd => sd.SubTotal)
                })
                .OrderByDescending(x => x.quantity)
                .Take(5)
                .Cast<object>()
                .ToListAsync();
        }
    }
}