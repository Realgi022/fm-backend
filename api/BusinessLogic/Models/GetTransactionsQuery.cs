using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class GetTransactionsQuery
    {
        [RegularExpression("^(income|expense)$", ErrorMessage = "Type must be either 'income' or 'expense'.")]
        public string? Type { get; set; }

        public string? Category { get; set; }

        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])$", ErrorMessage = "Period must be in format YYYY-MM.")]
        public string? Period { get; set; }
    }
}