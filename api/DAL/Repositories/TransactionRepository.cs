using DAL.Entities;
using DAL.Enum;
using DAL.Interfaces;
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

        public async Task<decimal> GetCurrentBalanceAsync(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .SumAsync(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<List<Transaction>> GetFilteredAsync(int userId, TransactionType? type, string? category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId)
                .AsQueryable();

            if (type.HasValue)
            {
                query = query.Where(t => t.Type == type.Value);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(t => t.Category.ToLower() == category.ToLower());
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.Date < endDate.Value);
            }

            return await query
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }
        public async Task<Transaction?> GetByIdAsync(int transactionId, int userId)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);
        }

        public async Task<Transaction?> UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return await _context.Transactions.FindAsync(transaction.Id);
        }

        public async Task DeleteAsync(int transactionId, int userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

            if (transaction == null)
            {
                throw new Exception("Transaction not found or access denied.");
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}