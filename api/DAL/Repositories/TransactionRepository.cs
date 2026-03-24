using DAL.Interfaces;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetByUserAndPeriodAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId &&
                            t.Date >= startDate &&
                            t.Date < endDate)
                .ToListAsync();
        }

        public async Task<double> GetCurrentBalanceAsync(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .SumAsync(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
        }
    }
}
