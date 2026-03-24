using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IBudgetRepository
    {
        Task<Budget?> GetMonthlyBudgetAsync(int userId, int year, int month);
    }
}
