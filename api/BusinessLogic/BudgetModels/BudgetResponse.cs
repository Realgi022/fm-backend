namespace BusinessLogic.BudgetModels
{
    public class BudgetResponse
    {
        public string Period { get; set; } = string.Empty;
        public double Limit { get; set; }
        public double Spent { get; set; }
        public double Remaining { get; set; }
        public int ProgressPercentage { get; set; }
    }
}
