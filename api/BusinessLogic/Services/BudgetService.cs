using BusinessLogic.BudgetModels;
using BusinessLogic.Interfaces;
using DAL.Enum;
using DAL.Interfaces;
using DAL.Entities;

namespace BusinessLogic.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ITransactionRepository _transactionRepository;

        public BudgetService(
            IBudgetRepository budgetRepository,
            ITransactionRepository transactionRepository)
        {
            _budgetRepository = budgetRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<BudgetResponse> CreateOrUpdateBudgetAsync(int userId, BudgetRequest request)
        {
            var calculation = await CalculateBudgetAsync(userId, request.Period);

            if (request.Limit < calculation.Spent)
            {
                throw new ArgumentException("Budget limit must be greater than or equal to current spending.");
            }

            var budget = await _budgetRepository.GetMonthlyBudgetAsync(userId, calculation.Year, calculation.Month);

            if (budget == null)
            {
                budget = new Budget
                {
                    UserId = userId,
                    Year = calculation.Year,
                    Month = calculation.Month,
                    Limit = request.Limit,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _budgetRepository.AddMonthlyBudgetAsync(budget);
            }
            else
            {
                budget.Limit = request.Limit;
                budget.UpdatedAt = DateTime.UtcNow;

                await _budgetRepository.UpdateMonthlyBudgetAsync(budget);
            }

            double remaining = request.Limit - calculation.Spent;
            int progressPercentage = request.Limit == 0
                ? 0
                : (int)Math.Round((calculation.Spent / request.Limit) * 100);

            return new BudgetResponse
            {
                Period = calculation.Period,
                Limit = request.Limit,
                Spent = calculation.Spent,
                Remaining = remaining,
                ProgressPercentage = progressPercentage
            };
        }


        public async Task<BudgetResponse?> GetMonthlyBudgetAsync(int userId, string? period)
        {
            var calculation = await CalculateBudgetAsync(userId, period);

            var budget = await _budgetRepository.GetMonthlyBudgetAsync(userId, calculation.Year, calculation.Month);

            double limit = budget?.Limit ?? 0;
            double remaining = limit - calculation.Spent;
            int progressPercentage = limit == 0
                ? 0
                : (int)Math.Round((calculation.Spent / limit) * 100);

            return new BudgetResponse
            {
                Period = calculation.Period,
                Limit = limit,
                Spent = calculation.Spent,
                Remaining = remaining,
                ProgressPercentage = progressPercentage
            };
        }

        private async Task<BudgetCalculationResult> CalculateBudgetAsync(int userId, string? requestPeriod)
        {
            string period = string.IsNullOrWhiteSpace(requestPeriod)
                ? DateTime.UtcNow.ToString("yyyy-MM")
                : requestPeriod;

            if (!DateTime.TryParse(period + "-01", out DateTime parsedDate))
            {
                throw new ArgumentException("Invalid period format. Use yyyy-MM.");
            }

            DateTime startDate = new DateTime(parsedDate.Year, parsedDate.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var transactions = await _transactionRepository.GetByUserAndPeriodAsync(userId, startDate, endDate);

            double spent = transactions
                .Where(t => t.Type == TransactionType.Expense &&
                            t.Date >= startDate &&
                            t.Date < endDate)
                .Sum(t => t.Amount);

            return new BudgetCalculationResult
            {
                Period = period,
                Year = parsedDate.Year,
                Month = parsedDate.Month,
                Spent = spent
            };
        }
    }
}
