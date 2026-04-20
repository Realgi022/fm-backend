using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class ReportSummaryRequest
    {
        [Required]
        public string Period { get; set; } = string.Empty;
    }
}