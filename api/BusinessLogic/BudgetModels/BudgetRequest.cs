using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.BudgetModels
{
    public class BudgetRequest
    {
        [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "Period must be in yyyy-MM format.")]
        public string? Period { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Limit { get; set; }
    }
}
