using Microsoft.AspNetCore.Http;

namespace DAL.Interfaces
{
    public interface IAIRepository
    {
        Task<string> ExtractTextFromReceipt(string prompt, IFormFile receipt);
    }
}