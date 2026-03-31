using Api.Controllers;
using Api.DTOs;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Register_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            var request = new RegisterRequest();

            _controller.ModelState.AddModelError("Name", "Name is required");

            var result = await _controller.Register(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Register_WhenSuccessful_Returns201()
        {
            var request = new RegisterRequest
            {
                Name = "Konstantin",
                Email = "test@test.com",
                Password = "Password123",
                Country = "Netherlands",
                PreferredCurrency = "EUR"
            };

            var response = new AuthResponse
            {
                Token = "fake-jwt-token",
                ExpiresIn = 3600,
                Message = "User successfully registered"
            };

            _authServiceMock
                .Setup(x => x.RegisterAsync(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.Country,
                    request.PreferredCurrency))
                .ReturnsAsync(response);

            var result = await _controller.Register(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);

            var value = Assert.IsType<AuthResponse>(objectResult.Value);
            Assert.Equal("User successfully registered", value.Message);
        }

        [Fact]
        public async Task Register_WhenEmailAlreadyExists_Returns409()
        {
            var request = new RegisterRequest
            {
                Name = "Konstantin",
                Email = "test@test.com",
                Password = "Password123",
                Country = "Netherlands",
                PreferredCurrency = "EUR"
            };

            _authServiceMock
                .Setup(x => x.RegisterAsync(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.Country,
                    request.PreferredCurrency))
                .ThrowsAsync(new EmailAlreadyExistsException());

            var result = await _controller.Register(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(409, objectResult.StatusCode);
        }

        [Fact]
        public async Task Register_WhenUnexpectedException_Returns500()
        {
            var request = new RegisterRequest
            {
                Name = "Konstantin",
                Email = "test@test.com",
                Password = "Password123",
                Country = "Netherlands",
                PreferredCurrency = "EUR"
            };

            _authServiceMock
                .Setup(x => x.RegisterAsync(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.Country,
                    request.PreferredCurrency))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.Register(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task Login_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            var request = new LoginRequest();

            _controller.ModelState.AddModelError("Email", "Email is required");

            var result = await _controller.Login(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Login_WhenSuccessful_Returns200()
        {
            var request = new LoginRequest
            {
                Email = "test@test.com",
                Password = "Password123"
            };

            var response = new AuthResponse
            {
                Token = "fake-jwt-token",
                ExpiresIn = 3600,
                Message = "Authentication successful"
            };

            _authServiceMock
                .Setup(x => x.LoginAsync(request.Email, request.Password))
                .ReturnsAsync(response);

            var result = await _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var value = Assert.IsType<AuthResponse>(okResult.Value);
            Assert.Equal("Authentication successful", value.Message);
        }

        [Fact]
        public async Task Login_WhenCredentialsAreInvalid_Returns401()
        {
            var request = new LoginRequest
            {
                Email = "test@test.com",
                Password = "wrongpassword"
            };

            _authServiceMock
                .Setup(x => x.LoginAsync(request.Email, request.Password))
                .ThrowsAsync(new InvalidCredentialsException());

            var result = await _controller.Login(request);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task Login_WhenUnexpectedException_Returns500()
        {
            var request = new LoginRequest
            {
                Email = "test@test.com",
                Password = "Password123"
            };

            _authServiceMock
                .Setup(x => x.LoginAsync(request.Email, request.Password))
                .ThrowsAsync(new Exception("Unexpected failure"));

            var result = await _controller.Login(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}