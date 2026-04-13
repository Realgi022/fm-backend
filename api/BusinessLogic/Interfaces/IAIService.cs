using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Interfaces
{
    public interface IAIService
    {
        Task<ReceiptScanResult> ExtractTextFromReceipt(IFormFile file, string documentType);
    }
}