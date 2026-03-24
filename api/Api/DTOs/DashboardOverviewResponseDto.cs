namespace Api.DTOs
{
    public class DashboardOverviewResponseDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty; // e.g. EUR
        public string Period { get; set; } = string.Empty;   // yyyy-MM

        public DashboardSummaryDto Summary { get; set; } = new();
        public MonthlyBudgetDto MonthlyBudget { get; set; } = new();
    }
}
