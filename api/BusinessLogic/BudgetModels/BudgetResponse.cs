namespace BusinessLogic.BudgetModels
{
    public class BudgetResponse
    {
        public string Period { get; set; } = string.Empty;
        public decimal Limit { get; set; }
        public decimal Spent { get; set; }
        public decimal Remaining { get; set; }
        public int ProgressPercentage { get; set; }
    }
}
