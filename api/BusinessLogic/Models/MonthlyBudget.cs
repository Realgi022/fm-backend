using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class MonthlyBudget
    {
        [Required]
        public double Spent { get; set; }
        [Required]
        public double Limit { get; set; }
        [Required]
        public double Remaining { get; set; }
        [Required]
        public int ProgressPercentage { get; set; }
    }
}
