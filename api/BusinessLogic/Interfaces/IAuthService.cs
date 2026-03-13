using DAL.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(string name, string email, string password, string country, string preferredCurrency);
        Task<User> LoginAsync(string email, string password);
    }
}