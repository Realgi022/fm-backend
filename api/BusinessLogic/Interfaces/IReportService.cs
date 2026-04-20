using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    public interface IReportService
    {
        Task<ReportSummaryResponse> GetSummaryAsync(int userId, string period);
    }
}