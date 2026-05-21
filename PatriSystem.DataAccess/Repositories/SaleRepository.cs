using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;

namespace PatriSystem.DataAccess.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly PatriSystemDbContext _context;

        public SaleRepository(PatriSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Sale sale, List<Product> products)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var detail in sale.SaleDetails)
                {
                    var product = products.First(p => p.Id == detail.ProductId);
                    product.CurrentStock -= detail.Quantity;
                }

                await _context.Sales.AddAsync(sale);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return sale.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            return await _context.Sales
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}