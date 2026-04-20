using System.ComponentModel.DataAnnotations;
using DAL.Enum;

namespace BusinessLogic.Models
{
    public class CreateTransactionRequest
    {
        [Required]
        [RegularExpression("^(income|expense)$", ErrorMessage = "Type must be either 'income' or 'expense'.")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("^(EUR|USD|GBP|CZK|PLN)$", ErrorMessage = "Invalid currency code")]
        public string? Currency { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Category { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format yyyy-MM-dd")]
        public string Date { get; set; } = string.Empty;
    }
}