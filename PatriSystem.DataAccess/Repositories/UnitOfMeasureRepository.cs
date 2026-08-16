using Microsoft.EntityFrameworkCore;
using PatriSystem.DataAccess.Context;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Repositories;

namespace PatriSystem.DataAccess.Repositories
{
    public class UnitOfMeasureRepository : IUnitOfMeasureRepository
    {
        private readonly PatriSystemDbContext _context;

        public UnitOfMeasureRepository(PatriSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<UnitOfMeasure>> GetAllAsync()
        {
            return await _context.UnitsOfMeasure.ToListAsync();
        }

        public async Task CreateAsync(UnitOfMeasure unitOfMeasure)
        {
            await _context.UnitsOfMeasure.AddAsync(unitOfMeasure);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            return await _context.UnitsOfMeasure
                .AnyAsync(u => u.Name.ToLower() == name.ToLower());
        }

        public async Task<UnitOfMeasure?> GetByNameAsync(string name)
        {
            return await _context.UnitsOfMeasure
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
        }

        public async Task<UnitOfMeasure?> GetByIdAsync(Guid id)
        {
            return await _context.UnitsOfMeasure
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}