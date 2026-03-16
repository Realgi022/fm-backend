using Api.DTOs;
using BusinessLogic.Exceptions;
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
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    throw new InvalidSyntaxException();
                }
                var user = await _authService.RegisterAsync(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.Country,
                    request.PreferredCurrency
                );

                var response = new AuthResponseDto
                {
                    Token = "fake-jwt-token",
                    ExpiresIn = 3600,
                    Message = "User successfully registered"
                };

                return StatusCode(201, response);

            }
            catch (InvalidSyntaxException ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
            catch (EmailAlreadyExistsException ex)
            {
                return StatusCode(409, new
                {
                    message = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.");
            }
        }

        // POST: /auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                if (loginRequestDto == null)
                {
                    throw new InvalidSyntaxException();
                }
                if (string.IsNullOrWhiteSpace(loginRequestDto.Email) || !loginRequestDto.Email.Contains("@") || string.IsNullOrWhiteSpace(loginRequestDto.Password))
                {
                    throw new InvalidCredentialsException();
                }
                var user = await _authService.LoginAsync(loginRequestDto.Email, loginRequestDto.Password);

                var response = new AuthResponseDto
                {
                    Token = "fake-jwt-token",
                    ExpiresIn = 3600,
                    Message = "Authentication successful"
                };

                return StatusCode(200, response);
            }
            catch (InvalidSyntaxException ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return StatusCode(401, new
                {
                    message = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "The server encountered an unexpected condition that prevented it from fulfilling the request."
                });
            }
        }
    }
}