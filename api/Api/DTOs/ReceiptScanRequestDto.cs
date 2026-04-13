using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class ReceiptScanRequestDto
    {
        [Required]
        public IFormFile File { get; set; } = default!;

        [Required]
        [RegularExpression("receipt|invoice", ErrorMessage = "documentType must be 'receipt' or 'invoice'.")]
        public string DocumentType { get; set; } = string.Empty;
    }
}