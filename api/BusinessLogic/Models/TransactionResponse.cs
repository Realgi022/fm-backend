namespace BusinessLogic.Models
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}