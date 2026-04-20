using DAL.Entities;
using DAL.Enum;

namespace DAL.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetByUserAndPeriodAsync(int userId, DateTime startDate, DateTime endDate);
        Task<decimal> GetCurrentBalanceAsync(int userId);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<List<Transaction>> GetFilteredAsync(int userId, TransactionType? type, string? category, DateTime? startDate, DateTime? endDate);
        Task<Transaction?> GetByIdAsync(int transactionId, int userId);
        Task<Transaction?> UpdateAsync(Transaction transaction);
        Task DeleteAsync(int transactionId, int userId);
    }
}
