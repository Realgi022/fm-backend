using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DAL.Entities;
using DAL.Interfaces;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(string name, string email, string password, string country, string preferredCurrency)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException();
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            User user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                Country = country,
                PreferredCurrency = preferredCurrency,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);

            var token = _jwtTokenService.GenerateToken(createdUser);
            var expiresIn = _jwtTokenService.GetTokenExpirationInSeconds();
            var response = new AuthResponseDto
            {
                Token = token,
                ExpiresIn = expiresIn,
                Message = "User successfully registered"
            };

            return response;
        }

        public async Task<AuthResponseDto> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email) ?? throw new InvalidCredentialsException();
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new InvalidCredentialsException();
            }

            var token = _jwtTokenService.GenerateToken(user);
            var expiresIn = _jwtTokenService.GetTokenExpirationInSeconds();
            var response = new AuthResponseDto
            {
                Token = token,
                ExpiresIn = expiresIn,
                Message = "Authentication successful"
            };

            return response;
        }
    }
}