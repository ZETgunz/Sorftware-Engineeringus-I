//Wusing backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypingController : ControllerBase
    {
        private static readonly Random rand = new Random();

        [HttpGet]
        public ActionResult GetText()
        {
            string text = "test melons please ignore";
            return Ok(text);
        }
    }
}
