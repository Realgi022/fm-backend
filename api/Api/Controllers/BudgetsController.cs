using BusinessLogic.BudgetModels;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("budgets")]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBudget([FromBody] BudgetRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Unauthorized. Missing or invalid JWT token." });
            }

            try
            {
                var result = await _budgetService.CreateOrUpdateBudgetAsync(userId, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new
                {
                    message = "The server encountered an unexpected condition that prevented it from fulfilling the request."
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMonthlyBudget([FromQuery] string? period)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Unauthorized. Missing or invalid JWT token." });
            }

            try
            {
                var result = await _budgetService.GetMonthlyBudgetAsync(userId, period);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new
                {
                    message = "The server encountered an unexpected condition that prevented it from fulfilling the request."
                });
            }
        }
    }
}
