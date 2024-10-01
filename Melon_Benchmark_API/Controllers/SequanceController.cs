using Melon_Benchmark_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Melon_Benchmark_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SequanceController : ControllerBase
    {
        private static readonly Random rand = new Random();

        [HttpGet]
        public ActionResult<IEnumerable<int>> GetCell()
        {
            return Ok(new SequanceCell { row = rand.Next() % 3, column = rand.Next() % 3 });
        }
    }

}