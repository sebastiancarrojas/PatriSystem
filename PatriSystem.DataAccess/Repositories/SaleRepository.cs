using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.DataAcess.Pagination;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;
using PatriSystem.Domain.Pagination;

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
                foreach (var detail in sale.SaleDetails.Where(d => !d.IsTemporary))
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

        public async Task<PaginationResponse<Sale>> GetPaginatedAsync(SalePaginationRequest request)
        {
            var queryable = _context.Sales
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();
                queryable = queryable.Where(s =>
                    s.SaleDetails.Any(sd =>
                        sd.ProductName != null && sd.ProductName.ToLower().Contains(filter) ||
                        sd.Product != null && sd.Product.ProductName.ToLower().Contains(filter)));
            }

            if (request.StartDate.HasValue)
                queryable = queryable.Where(s => s.SaleDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                queryable = queryable.Where(s => s.SaleDate <= request.EndDate.Value);

            queryable = queryable.OrderByDescending(s => s.SaleDate);

            var pagedList = await PagedList<Sale>.ToPagedListAsync(queryable, request);

            return new PaginationResponse<Sale>
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