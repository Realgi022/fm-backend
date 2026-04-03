using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DAL.Interfaces
{
    public interface IAIRepository
    {
        Task<string> ExtractTextFromReceipt(string prompt, IFormFile receipt);
    }
}
