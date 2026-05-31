using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.DataAcess.Pagination;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Pagination;

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

        public async Task<Brand?> GetByNameAsync(string name)
        {
            return await _context.Brands
                .FirstOrDefaultAsync(b => b.BrandName.ToLower() == name.ToLower());
        }

        public async Task UpdateAsync (Brand brand)
        {
            brand.UpdatedAt = DateTime.UtcNow;
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<Brand?> GetByIdAsync(Guid id)
        {
            return await _context.Brands
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<PaginationResponse<Brand>> GetPaginatedAsync(BrandPaginationRequest request)
        {
            var queryable = _context.Brands.AsNoTracking();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                string filter = request.Filter.ToLower();
                queryable = queryable.Where(b =>
                b.BrandName.ToLower().Contains(request.Filter.ToLower()));
            }

            var pagedList = await PagedList<Brand>.ToPagedListAsync(queryable, request);

            return new PaginationResponse<Brand>
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