using BCrypt.Net;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
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
            var registerRequest = new RegisterRequest
            {
                Name = name,
                Email = email,
                Password = password,
                Country = country,
                PreferredCurrency = preferredCurrency
            };

            ValidateRegisterRequest(registerRequest);

            var existingUser = await _userRepository.GetByEmailAsync(registerRequest.Email);

            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException();
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
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            ValidateLoginRequest(loginRequest);

            var user = await _userRepository.GetByEmailAsync(email) ?? throw new InvalidCredentialsException();
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new InvalidCredentialsException();
            }
            return user;
        }

        public void ValidateRegisterRequest(RegisterRequest request)
        {
            if (request == null)
            {
                throw new InvalidSyntaxException();
            }
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                !request.Email.Contains("@") ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Country) ||
                string.IsNullOrWhiteSpace(request.PreferredCurrency))
            {
                throw new InvalidSyntaxException();
            }
        }
        public void ValidateLoginRequest(LoginRequest request)
        {
            if (request == null)
            {
                throw new InvalidSyntaxException();
            }
            if (!request.Email.Contains("@") ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                throw new InvalidSyntaxException();
            }
        }
    }
}