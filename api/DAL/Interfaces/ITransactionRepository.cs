using DAL.Entities;
using DAL.Enum;

namespace DAL.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetByUserAndPeriodAsync(int userId, DateTime startDate, DateTime endDate);
        Task<double> GetCurrentBalanceAsync(int userId);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<List<Transaction>> GetFilteredAsync(int userId, TransactionType? type, string? category, DateTime? startDate, DateTime? endDate);
    }
}
