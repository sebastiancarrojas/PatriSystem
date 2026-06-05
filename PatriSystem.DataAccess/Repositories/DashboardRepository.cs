using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.Domain.DTOs.Dashboard;
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

        private static DateTime ToUtc(DateTime local) => local.ToUniversalTime();

        public async Task<int> GetSalesTodayCountAsync()
        {
            var start = ToUtc(DateTime.Now.Date);
            var end = ToUtc(DateTime.Now.Date.AddDays(1));
            return await _context.Sales.CountAsync(s => s.SaleDate >= start && s.SaleDate < end);
        }

        public async Task<decimal> GetSalesTodayAmountAsync()
        {
            var start = ToUtc(DateTime.Now.Date);
            var end = ToUtc(DateTime.Now.Date.AddDays(1));
            return await _context.Sales
                .Where(s => s.SaleDate >= start && s.SaleDate < end)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;
        }

        public async Task<int> GetSalesWeekCountAsync()
        {
            var start = ToUtc(DateTime.Now.Date.AddDays(-6));
            return await _context.Sales.CountAsync(s => s.SaleDate >= start);
        }

        public async Task<decimal> GetSalesWeekAmountAsync()
        {
            var start = ToUtc(DateTime.Now.Date.AddDays(-6));
            return await _context.Sales
                .Where(s => s.SaleDate >= start)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;
        }

        public async Task<int> GetSalesMonthCountAsync()
        {
            var now = DateTime.Now;
            var start = ToUtc(new DateTime(now.Year, now.Month, 1));
            return await _context.Sales.CountAsync(s => s.SaleDate >= start);
        }

        public async Task<decimal> GetSalesMonthAmountAsync()
        {
            var now = DateTime.Now;
            var start = ToUtc(new DateTime(now.Year, now.Month, 1));
            return await _context.Sales
                .Where(s => s.SaleDate >= start)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;
        }

        public async Task<int> GetTotalProductsAsync()
            => await _context.Products.CountAsync();

        public async Task<IEnumerable<DailySalesResponseDto>> GetLast7DaysSalesAsync()
        {
            var start = ToUtc(DateTime.Now.Date.AddDays(-6));
            var sales = await _context.Sales
                .Where(s => s.SaleDate >= start)
                .ToListAsync();

            return sales
                .GroupBy(s => s.SaleDate.ToLocalTime().Date)
                .Select(g => new DailySalesResponseDto
                {
                    Date = g.Key,
                    Amount = g.Sum(s => s.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

        public async Task<IEnumerable<TopProductResponseDto>> GetTopProductsLastMonthAsync()
        {
            var now = DateTime.Now;
            var start = ToUtc(new DateTime(now.Year, now.Month, 1));
            return await _context.SaleDetails
                .Where(sd => sd.Sale.SaleDate >= start && sd.ProductName != null)
                .GroupBy(sd => sd.ProductName)
                .Select(g => new TopProductResponseDto
                {
                    ProductName = g.Key!,
                    Quantity = g.Sum(sd => sd.Quantity),
                    Revenue = g.Sum(sd => sd.SubTotal)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToListAsync();
        }
    }
}