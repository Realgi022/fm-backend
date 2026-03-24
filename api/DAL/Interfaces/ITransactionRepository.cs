using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetByUserAndPeriodAsync(int userId, DateTime startDate, DateTime endDate);
        Task<double> GetCurrentBalanceAsync(int userId);
    }
}
