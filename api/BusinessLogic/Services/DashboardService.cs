using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DAL.Entities;
using DAL.Interfaces;

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
                throw new Exception("User not found.");

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

            var budgetLimit = budget?.Amount ?? 0;
            var remaining = budgetLimit - expenses;
            var progressPercentage = budgetLimit <= 0
                ? 0
                : (int)Math.Round((expenses / budgetLimit) * 100);

            if (progressPercentage > 100)
                progressPercentage = 100;

            return new DashboardOverviewResponse
            {
                UserName = user.Name,
                Currency = user.PreferredCurrency,
                Period = now.ToString("yyyy-MM"),
                Summary = new DashboardSummary
                {
                    TotalBalance = totalBalance,
                    Income = income,
                    Expenses = expenses
                },
                MonthlyBudget = new MonthlyBudget
                {
                    Spent = expenses,
                    Limit = budgetLimit,
                    Remaining = remaining,
                    ProgressPercentage = progressPercentage
                }
            };
        }
    }
}
