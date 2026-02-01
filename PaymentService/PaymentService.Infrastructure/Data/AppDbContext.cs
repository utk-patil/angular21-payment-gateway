using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.OrderId).IsUnique();

                entity.Property(x => x.Amount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Status)
                      .HasConversion<string>();

                entity.Property(x => x.CreatedOn)
                      .IsRequired();
            });
        }
    }
}
