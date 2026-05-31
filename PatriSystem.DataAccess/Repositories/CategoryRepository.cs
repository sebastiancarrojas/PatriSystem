using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.DataAcess.Pagination;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Pagination;

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

        public async Task UpdateAsync (Category category) {
            category.UpdatedAt = DateTime.UtcNow;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<PaginationResponse<Category>> GetPaginatedAsync(CategoryPaginationRequest request)
        {
            var queryable = _context.Categories.AsNoTracking();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                string filter = request.Filter.ToLower();
                queryable = queryable.Where(c =>
                c.CategoryName.ToLower().Contains(request.Filter.ToLower()));
            }

            var pagedList = await PagedList<Category>.ToPagedListAsync(queryable, request);

            return new PaginationResponse<Category>
            {
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                RecordsPerPage = pagedList.RecordsPerPage,
                TotalCount = pagedList.TotalCount,
                Filter = request.Filter,
                Items = pagedList.ToList()
            };
        }
    }
}