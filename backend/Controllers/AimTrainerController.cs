using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace AimTrainerAPI.Controllers
{
    [ApiController]
    [Route("api/aimtrainer")]
    public class AimTrainerController : ControllerBase
    {
        private readonly Random _random = new Random();


        [HttpPost("target")]
        public IActionResult GetTarget([FromBody] Target maxTarget)
        {
            if (maxTarget.x <= 0 || maxTarget.y <= 0)
            {
                return BadRequest("Invalid dimensions provided.");
            }

            int targetSize = 50; // Target size in pixels

            // Ensure the target stays within bounds
            int x = _random.Next(0, maxTarget.x - targetSize);
            int y = _random.Next(0, maxTarget.y - targetSize);

            return Ok(new Target {x = x, y = y});
        }
    }
}