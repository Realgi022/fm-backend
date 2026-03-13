using BCrypt.Net;
using BusinessLogic.Interfaces;
using DAL.Entities;
using DAL.Repositories;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterAsync(string name, string email, string password, string country, string preferredCurrency)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                Country = country,
                PreferredCurrency = preferredCurrency,
                CreatedAt = DateTime.UtcNow
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }
            return user;
        }
    }
}