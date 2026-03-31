using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int userId);
        Task<User> CreateAsync(User user);
    }
}