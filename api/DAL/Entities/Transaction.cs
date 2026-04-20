using DAL.Enum;

namespace DAL.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}