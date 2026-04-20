using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class DashboardSummary
    {
        [Required]
        public decimal TotalBalance { get; set; }
        [Required]
        public decimal Income { get; set; }
        [Required]
        public decimal Expenses { get; set; }
    }
}
