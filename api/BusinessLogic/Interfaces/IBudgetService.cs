using BusinessLogic.BudgetModels;

namespace BusinessLogic.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetResponse> CreateOrUpdateBudgetAsync(int userId, BudgetRequest request);
        Task<BudgetResponse?> GetMonthlyBudgetAsync(int userId, string? period);
    }
}
