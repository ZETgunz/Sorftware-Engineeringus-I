using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SequenceController : ControllerBase
    {
        private static readonly Random rand = new Random();

        [HttpGet]
        public ActionResult<IEnumerable<int>> GetCell()
        {
            return Ok(new SequenceCell { row = rand.Next() % 3, column = rand.Next() % 3 });
        }
    }

}