using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace BusinessLogic.Services
{
    public class AIService : IAIService
    {
        private readonly IAIRepository _aiRepository;

        public AIService(IAIRepository aiRepository)
        {
            _aiRepository = aiRepository;
        }

        public async Task<ReceiptScanResult> ExtractTextFromReceipt(IFormFile file)
        {
            string prompt = "You are an AI that extracts structured receipt data.\r\n\r\nReturn ONLY a valid JSON object.\r\nDo not add markdown.\r\nDo not wrap the response in ```json.\r\nDo not add explanation text.\r\nDo not add comments.\r\nDo not add any text before or after the JSON.\r\n\r\nUse exactly this structure:\r\n\r\n{\r\n  \"scanId\": \"scan_001\",\r\n  \"status\": \"partial\",\r\n  \"detectedFields\": [\"amount\", \"date\", \"merchant\"],\r\n  \"suggestedType\": \"expense\",\r\n  \"amount\": 23.5,\r\n  \"amountCandidates\": [20, 3, 23],\r\n  \"currency\": \"EUR\",\r\n  \"merchant\": \"Lidl\",\r\n  \"suggestedCategory\": \"Food\",\r\n  \"description\": \"Grocery purchase\",\r\n  \"date\": \"2026-03-10\",\r\n  \"confidence\": {\r\n    \"amount\": 0.95,\r\n    \"date\": 0.8,\r\n    \"merchant\": 0.7\r\n  },\r\n  \"rawText\": \"LIDL TOTAL 23.50 EUR 19/03/2026\"\r\n}\r\n\r\nRules:\r\n- amount must be a number\r\n- amountCandidates must be an array of numbers\r\n- date must be in YYYY-MM-DD format or null\r\n- confidence must contain numeric values from 0 to 1\r\n- detectedFields must list only the fields actually detected\r\n- status must be one of: success, partial, failed\r\n- suggestedType must be either income or expense\r\n- currency must be a 3-letter currency code like EUR or USD\r\n- rawText must contain the OCR text";

            var responseText = await _aiRepository.ExtractTextFromReceipt(prompt, file);

            var cleaned = responseText
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            var result = JsonSerializer.Deserialize<ReceiptScanResult>(cleaned, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
            {
                throw new Exception("Failed to parse Gemini response.");
            }

            return result;
        }


    }
}
