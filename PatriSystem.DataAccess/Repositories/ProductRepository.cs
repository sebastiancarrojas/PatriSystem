using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;

namespace PatriSystem.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PatriSystemDbContext _context;

        public ProductRepository(PatriSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsWithBarcodeAsync(string barcode)
        {
            return await _context.Products
                .AnyAsync(p => p.Barcode == barcode);
        }
    }
}