using DAL.Interfaces;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AIRepository : IAIRepository
    {
        private readonly Client _client;

        public AIRepository(string apiKey)
        {
            _client = new Client(apiKey: apiKey);
        }

        public async Task<string> ExtractTextFromReceipt(string prompt, IFormFile receipt)
        {
            using var ms = new MemoryStream();
            await receipt.CopyToAsync(ms);
            var imageBytes = ms.ToArray();

            var parts = new List<Part>
    {
        Part.FromText(prompt),
        Part.FromBytes(imageBytes, receipt.ContentType)
    };
            
            var contents = new List<Content>
            {
               new Content {Parts= parts }
            };


            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-3.1-flash-lite-preview",
                contents: contents
            );

            return response.Text;
        }
    }
}
