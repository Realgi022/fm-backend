using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("scan")]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("receipt")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ExtractTextFromReceipt(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var extractedText = await _aiService.ExtractTextFromReceipt(file);
            return Ok(new { Text = extractedText });
        }
    }
}