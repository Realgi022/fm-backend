using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class DashboardSummary
    {
        [Required]
        public double TotalBalance { get; set; }
        [Required]
        public double Income { get; set; }
        [Required]
        public double Expenses { get; set; }
    }
}
