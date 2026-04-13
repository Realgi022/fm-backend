using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class ConfirmScannedTransactionRequest
    {
        [Required]
        public string ScanId { get; set; } = string.Empty;

        [Required]
        [RegularExpression("income|expense", ErrorMessage = "type must be 'income' or 'expense'.")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "amount must be greater than 0.")]
        public double Amount { get; set; }

        public string? Currency { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}