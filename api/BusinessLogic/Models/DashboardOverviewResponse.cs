namespace BusinessLogic.Models
{
    public class DashboardOverviewResponse
    {
        public string UserName { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty; // e.g. EUR
        public string Period { get; set; } = string.Empty;   // yyyy-MM

        public DashboardSummary Summary { get; set; } = new();
        public MonthlyBudget MonthlyBudget { get; set; } = new();
    }
}
