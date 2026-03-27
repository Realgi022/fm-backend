using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class DashboardOverviewResponse
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Currency { get; set; } = string.Empty; 
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "Period must be in format YYYY-MM.")]
        public string Period { get; set; } = string.Empty;   // yyyy-MM
        [Required]
        public DashboardSummary Summary { get; set; } = new();
        [Required]
        public MonthlyBudget MonthlyBudget { get; set; } = new();
    }
}
