using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardOverviewResponse> GetDashboardOverviewAsync(int userId);
    }
}
