using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAIService
    {
        Task<ReceiptScanResult> ExtractTextFromReceipt(IFormFile file);
    }
}
