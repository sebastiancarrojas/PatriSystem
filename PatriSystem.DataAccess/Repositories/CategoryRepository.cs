using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;

namespace PatriSystem.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PatriSystemDbContext _context;

        public CategoryRepository(PatriSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            return await _context.Categories
                .AnyAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }
    }
}