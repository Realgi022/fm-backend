using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BusinessLogic.Services
{
    public class AIService : IAIService
    {
        private readonly IAIRepository _aiRepository;

        public AIService(IAIRepository aiRepository)
        {
            _aiRepository = aiRepository;
        }

        public async Task<ReceiptScanResult> ExtractTextFromReceipt(IFormFile file, string documentType)
        {
            string prompt = $@"
You are an AI that extracts structured {(documentType == "invoice" ? "invoice" : "receipt")} data.

Return ONLY a valid JSON object.
Do not add markdown.
Do not wrap the response in ```json.
Do not add explanation text.
Do not add comments.
Do not add any text before or after the JSON.

Use exactly this structure:

{{
  ""scanId"": ""scan_001"",
  ""status"": ""partial"",
  ""detectedFields"": [""amount"", ""date"", ""merchant""],
  ""suggestedType"": ""expense"",
  ""amount"": 23.5,
  ""amountCandidates"": [20, 3, 23],
  ""currency"": ""EUR"",
  ""merchant"": ""Lidl"",
  ""suggestedCategory"": ""Food"",
  ""description"": ""Grocery purchase"",
  ""date"": ""2026-03-10"",
  ""confidence"": {{
    ""amount"": 0.95,
    ""date"": 0.8,
    ""merchant"": 0.7
  }},
  ""rawText"": ""LIDL TOTAL 23.50 EUR 19/03/2026""
}}

Rules:
- amount must be a number or null
- amountCandidates must be an array of numbers
- date must be in YYYY-MM-DD format or null
- confidence must contain numeric values from 0 to 1
- detectedFields must list only the fields actually detected
- status must be one of: success, partial, failed
- suggestedType must be either income or expense
- rawText must contain the OCR text
- documentType is '{documentType}', so adapt extraction logic accordingly
- currency must be a 3-letter currency code like EUR or USD
- detect currency only from symbols and text visible on the receipt or invoice
- if the document contains € or EUR, return EUR
- if the document contains $ or USD, return USD
- if the document contains £ or GBP, return GBP
- do not guess USD by default
- if no explicit currency symbol or 3-letter currency code is visible in the document, return null for currency
- do not infer currency from merchant name, language, country, or assumptions
";

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

            if (!string.IsNullOrWhiteSpace(result.RawText))
            {
                var raw = result.RawText.ToUpperInvariant();

                if (raw.Contains("EUR") || raw.Contains("€"))
                {
                    result.Currency = "EUR";
                }
                else if (raw.Contains("USD") || raw.Contains("$"))
                {
                    result.Currency = "USD";
                }
                else if (raw.Contains("GBP") || raw.Contains("£"))
                {
                    result.Currency = "GBP";
                }
            }

            return result;
        }
    }
}