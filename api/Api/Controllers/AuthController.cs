using Api.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: /auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "The server could not understand the request due to invalid syntax."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "Name, email and password are required."
                });
            }

            try
            {
                var user = await _authService.RegisterAsync(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.Country,
                    request.PreferredCurrency
                );

                var response = new RegisterResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Country = user.Country,
                    PreferredCurrency = user.PreferredCurrency,
                    CreatedAt = user.CreatedAt
                };

                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // POST: /auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "The server could not understand the request due to invalid syntax."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "Email and password are required"
                });
            }

            try
            {
                var user = await _authService.LoginAsync(request.Email, request.Password);

                var response = new LoginResponse
                {
                    Message = "Login successful",
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }
    }
}