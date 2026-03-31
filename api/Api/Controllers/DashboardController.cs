using Api.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("dashboard")]
    [Authorize]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: /dashboard/summary
        [Authorize]
        [HttpGet("summary")]
        [ProducesResponseType(typeof(DashboardSummary),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardSummaryAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new ErrorResponse 
                { 
                    Message = "Unauthorized. Missing or invalid JWT token."
                });
            }

            try 
            {
                var result = await _dashboardService.GetDashboardOverviewAsync(userId);
                return Ok(result);
            }
            catch (InvalidCredentialException)
            {
                return Unauthorized(new ErrorResponse 
                { 
                    Message = "Unauthorized. Missing or invalid JWT token."
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request." });
            }
            
        }
    }
}