using Microsoft.EntityFrameworkCore;
using PatriSystem.Domain.Entities;

namespace PatriSystem.DataAccess.Context
{
    public class PatriSystemDbContext : DbContext
    {
        public PatriSystemDbContext(DbContextOptions<PatriSystemDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleDetail>()
                .Property(sd => sd.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleDetail>()
                .Property(sd => sd.SubTotal)
                .HasPrecision(18, 2);
        }
    }
}