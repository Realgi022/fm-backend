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
        public double Amount { get; set; }

        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be a 3-letter uppercase code.")]
        public string? Currency { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Category { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}