using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ReceiptScanResult
    {
        public string ScanId { get; set; }
        public string Status { get; set; }
        public List<string> DetectedFields { get; set; }
        public string SuggestedType { get; set; }
        public decimal? Amount { get; set; }
        public List<decimal> AmountCandidates { get; set; }
        public string Currency { get; set; }
        public string Merchant { get; set; }
        public string SuggestedCategory { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public ConfidenceResult Confidence { get; set; }
        public string RawText { get; set; }
    }

    public class ConfidenceResult
    {
        public double Amount { get; set; }
        public double Date { get; set; }
        public double Merchant { get; set; }
    }
}
