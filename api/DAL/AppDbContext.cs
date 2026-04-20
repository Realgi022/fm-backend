using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Budget>()
                .Property(b => b.Limit)
                .HasPrecision(18, 2);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; internal set; }
        public DbSet<Budget> Budgets { get; internal set; }
    }
}
