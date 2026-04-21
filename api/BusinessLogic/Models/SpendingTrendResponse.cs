namespace BusinessLogic.Models
{
    public class SpendingTrendResponse
    {
        public string Period { get; set; } = string.Empty;
        public List<SpendingTrendItem> Items { get; set; } = new();
    }
}
