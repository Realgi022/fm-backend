namespace BusinessLogic.Models
{
    public class ReportSummaryResponse
    {
        public string Period { get; set; } = string.Empty;
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
    }
}