namespace BusinessLogic.Models
{
    public class ConfirmedTransactionResponse
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}