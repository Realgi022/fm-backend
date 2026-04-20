using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class TransactionResponse
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Currency { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public string? Description { get; set; }
        [Required]
        public string Date { get; set; } = string.Empty;
        [Required]
        public string CreatedAt { get; set; } = string.Empty;
        [Required]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}