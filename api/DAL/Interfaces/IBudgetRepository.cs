using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IBudgetRepository
    {
        Task<Budget?> GetMonthlyBudgetAsync(int userId, int year, int month);
        Task AddMonthlyBudgetAsync(Budget budget);
        Task UpdateMonthlyBudgetAsync(Budget budget);

    }
}
