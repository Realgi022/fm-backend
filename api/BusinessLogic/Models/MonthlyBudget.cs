using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class MonthlyBudget
    {
        public double Spent { get; set; }
        public double Limit { get; set; }
        public double Remaining { get; set; }
        public int ProgressPercentage { get; set; }
    }
}
