using BusinessLogic.BudgetModels;
using BusinessLogic.Interfaces;
using DAL.Enum;
using DAL.Interfaces;
using DAL.Entities;
using System.Globalization;

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

            decimal remaining = request.Limit - calculation.Spent;
            int? progressPercentage = request.Limit == 0m
                ? null
                : (int)Math.Round((calculation.Spent / request.Limit) * 100m);

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

            decimal limit = budget?.Limit ?? 0m;
            decimal remaining = limit - calculation.Spent;
            int? progressPercentage = limit == 0m
                ? null
                : (int)Math.Round((calculation.Spent / limit) * 100m);

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

            if (!DateTime.TryParseExact(
                    period,
                    "yyyy-MM",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                throw new ArgumentException("Invalid period format. Use yyyy-MM.");
            }

            DateTime startDate = new DateTime(parsedDate.Year, parsedDate.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var transactions = await _transactionRepository.GetByUserAndPeriodAsync(userId, startDate, endDate);

            decimal spent = transactions
                .Where(t => t.Type == TransactionType.Expense)
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