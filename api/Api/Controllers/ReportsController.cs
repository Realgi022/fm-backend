using Api.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("summary")]
        [ProducesResponseType(typeof(ReportSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSummary([FromQuery] ReportSummaryRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new ErrorResponse
                {
                    Message = "Unauthorized. Missing or invalid JWT token."
                });
            }

            try
            {
                var result = await _reportService.GetSummaryAsync(userId, request.Period);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message
                });
            }
            catch
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "The server encountered an unexpected condition that prevented it from fulfilling the request."
                });
            }
        }
    }
}