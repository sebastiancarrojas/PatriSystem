using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.DataAcess.Pagination;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Pagination;

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

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Status = false;
                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PaginationResponse<Product>> GetPaginatedAsync(ProductPaginationRequest request)
        {
            var queryable = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();
                queryable = queryable.Where(p =>
                    p.ProductName.ToLower().Contains(filter) ||
                    (p.Barcode != null && p.Barcode.ToLower().Contains(filter)) ||
                    p.Category.CategoryName.ToLower().Contains(filter) ||
                    p.Brand.BrandName.ToLower().Contains(filter));
            }

            if (request.CategoryId.HasValue)
                queryable = queryable.Where(p => p.CategoryId == request.CategoryId.Value);

            if (request.BrandId.HasValue)
                queryable = queryable.Where(p => p.BrandId == request.BrandId.Value);

            if (request.Status.HasValue)
                queryable = queryable.Where(p => p.Status == request.Status.Value);

            var pagedList = await PagedList<Product>.ToPagedListAsync(queryable, request);

            return new PaginationResponse<Product>
            {
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                RecordsPerPage = pagedList.RecordsPerPage,
                TotalCount = pagedList.TotalCount,
                Filter = request.Filter,
                Items = pagedList.ToList()
            };
        }

        public async Task<List<Product>> SearchForSaleAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<Product>();

            return await _context.Products
                .Where(p =>
                    p.Status == true &&
                    (p.ProductName.Contains(term) ||
                     (p.Barcode != null && p.Barcode.Contains(term))))
                .OrderBy(p => p.ProductName)
                .Take(10)
                .ToListAsync();
        }

        public async Task ActivateAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Status = true;
                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
    