using BusinessLogic.Models;
using DAL.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(string name, string email, string password, string country, string preferredCurrency);
        Task<AuthResponse> LoginAsync(string email, string password);
    }
}