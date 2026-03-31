using Api.DTOs;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
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
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "The server could not understand the request due to invalid syntax."
                });
            }

            try
            {
                var response = await _authService.RegisterAsync(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.Country,
                    request.PreferredCurrency
                );

                return StatusCode(StatusCodes.Status201Created, response);

            }
            catch (EmailAlreadyExistsException ex)
            {
                return StatusCode(409, new ErrorResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Message = "The server encountered an unexpected condition that prevented it from fulfilling the request."
                });
            }
        }

        // POST: /auth/login
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "The server could not understand the request due to invalid syntax."
                });
            }
            try
            {
                var response = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);
                return Ok(response);
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Message = "The server encountered an unexpected condition that prevented it from fulfilling the request."
                });
            }
        }
    }
}