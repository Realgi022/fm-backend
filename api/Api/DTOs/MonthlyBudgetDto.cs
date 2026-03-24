namespace Api.DTOs
{
    public class MonthlyBudgetDto
    {
        public double Spent { get; set; }
        public double Limit { get; set; }
        public double Remaining { get; set; }
        public int ProgressPercentage { get; set; }
    }
}
