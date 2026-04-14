namespace BusinessLogic.BudgetModels
{
    public class BudgetCalculationResult
    {
        public string Period { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Month { get; set; }
        public double Spent { get; set; }
    }
}
