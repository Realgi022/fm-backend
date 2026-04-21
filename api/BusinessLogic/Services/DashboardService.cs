using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DAL.Entities;
using DAL.Enum;
using DAL.Interfaces;
using System.Security.Authentication;

namespace BusinessLogic.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBudgetRepository _budgetRepository;

        public DashboardService(
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            IBudgetRepository budgetRepository)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _budgetRepository = budgetRepository;
        }

        public async Task<DashboardOverviewResponse> GetDashboardOverviewAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidCredentialException();

            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var transactions = await _transactionRepository.GetByUserAndPeriodAsync(userId, startOfMonth, endOfMonth);
            var budget = await _budgetRepository.GetMonthlyBudgetAsync(userId, now.Year, now.Month);

            var income = transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var expenses = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            var totalBalance = await _transactionRepository.GetCurrentBalanceAsync(userId);

            var budgetLimit = budget?.Limit ?? 0m;
            var remaining = Math.Max(0m, budgetLimit - expenses);

            decimal rawProgress = budgetLimit <= 0m
                ? 0m
                : (expenses / budgetLimit) * 100m;

            rawProgress = Math.Min(rawProgress, 100m);

            int progressPercentage = (int)Math.Round(rawProgress);

            return new DashboardOverviewResponse
            {
                UserName = user.Name,
                Currency = user.PreferredCurrency,
                Period = now.ToString("yyyy-MM"),
                Summary = new DashboardSummary
                {
                    TotalBalance = Math.Round(totalBalance, 2),
                    Income = Math.Round(income, 2),
                    Expenses = Math.Round(expenses, 2)
                },
                MonthlyBudget = new MonthlyBudget
                {
                    Spent = Math.Round(expenses, 2),
                    Limit = Math.Round(budgetLimit, 2),
                    Remaining = Math.Round(remaining, 2),
                    ProgressPercentage = progressPercentage
                }
            };
        }
    }
}
