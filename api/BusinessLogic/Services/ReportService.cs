using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DAL.Enum;
using DAL.Interfaces;

namespace BusinessLogic.Services
{
    public class ReportService : IReportService
    {
        private readonly ITransactionRepository _transactionRepository;

        public ReportService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ReportSummaryResponse> GetSummaryAsync(int userId, string period)
        {
            if (string.IsNullOrWhiteSpace(period))
            {
                throw new ArgumentException("Period is required.");
            }

            if (!DateTime.TryParse(period + "-01", out DateTime parsedDate))
            {
                throw new ArgumentException("Invalid period format. Use yyyy-MM.");
            }

            DateTime startDate = new DateTime(parsedDate.Year, parsedDate.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var transactions = await _transactionRepository.GetByUserAndPeriodAsync(userId, startDate, endDate);

            decimal income = transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            decimal expenses = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            return new ReportSummaryResponse
            {
                Period = period,
                Income = income,
                Expenses = expenses,
                Balance = income - expenses
            };
        }
        public async Task<SpendingTrendResponse> GetSpendingTrendsAsync(int userId, string period)
        {
            if (!DateTime.TryParse(period + "-01", out DateTime parsedDate))
            {
                throw new ArgumentException("Invalid period format. Use yyyy-MM.");
            }

            DateTime startDate = new DateTime(parsedDate.Year, parsedDate.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var transactions = await _transactionRepository
                .GetByUserAndPeriodAsync(userId, startDate, endDate);

            // Only expenses
            var expenses = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .ToList();

            int daysInMonth = DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month);

            var result = new List<SpendingTrendItem>();

            decimal cumulative = 0m;

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDate = new DateTime(parsedDate.Year, parsedDate.Month, day);

                decimal dailyTotal = expenses
                    .Where(t => t.Date.Date == currentDate.Date)
                    .Sum(t => t.Amount);

                cumulative += dailyTotal;

                result.Add(new SpendingTrendItem
                {
                    Date = currentDate,
                    CumulativeAmount = Math.Round(cumulative, 2)
                });
            }

            return new SpendingTrendResponse
            {
                Period = period,
                Items = result
            };
        }
    }
}