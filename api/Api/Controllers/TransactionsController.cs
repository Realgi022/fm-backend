using Api.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Globalization;

namespace Api.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "The server could not understand the request due to invalid syntax.",
                    errors = ModelState
                });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Missing or invalid JWT token." });
            }

            try
            {
                var createdTransaction = await _transactionService.CreateTransactionAsync(userId, request);

                return StatusCode(201, createdTransaction);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse { Message = "The server encountered an unexpected condition that prevented it from fullfilling the request" });
            }

        }

        [Authorize]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmScannedTransaction([FromBody] ConfirmScannedTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "The server could not understand the request due to invalid syntax."
                });
            }

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
                var confirmedTransaction = await _transactionService.ConfirmScannedTransactionAsync(request, userId);

                return StatusCode(201, confirmedTransaction);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "The server encountered an unexpected condition that prevented it from fullfilling the request"
                });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] GetTransactionsQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "The server could not understand the request due to invalid syntax."
                });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new ErrorResponse { Message = "Missing or invalid JWT token." });
            }

            try
            {
                var transactions = await _transactionService.GetTransactionsAsync(userId, query);

                return Ok(new
                {
                    items = transactions,
                    total = transactions.Count
                });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse { Message = "The server encountered an unexpected condition that prevented it from fullfilling the request" });
            }
        }
        [Authorize]
        [HttpPut("{transactionId}")]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] UpdateTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "The server could not understand the request due to invalid syntax."
                });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new ErrorResponse { Message = "Unauthorized. Missing or invalid JWT token." });
            }

            try
            {
                var transaction = await _transactionService.UpdateTransactionAsync(transactionId, userId, request);

                return Ok(transaction);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse { Message = "The server encountered an unexpected condition that prevented it from fullfilling the request" });
            }
        }
        [Authorize]
        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new ErrorResponse { Message = "Unauthorized. Missing or invalid JWT token." });
            }
            try
            {
                await _transactionService.DeleteTransactionAsync(transactionId, userId);
                return Ok(new { message = "Transaction deleted successfully" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse { Message = "The server encountered an unexpected condition that prevented it from fullfilling the request" });
            }
        }
    }
}
