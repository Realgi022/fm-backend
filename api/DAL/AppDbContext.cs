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
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; internal set; }
        public DbSet<Budget> Budgets { get; internal set; }
    }
}
