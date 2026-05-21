using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;

namespace PatriSystem.DataAccess.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly PatriSystemDbContext _context;

        public BrandRepository(PatriSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task CreateAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            return await _context.Brands
                .AnyAsync(b => b.BrandName.ToLower() == name.ToLower());
        }
    }
}