using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeekerController : ControllerBase
    {
        private static readonly Random rand = new Random();

        [HttpGet]
        public ActionResult<List<int>> GetCell()
        {
            List<int> cell = new List<int> { };
            cell.Add(rand.Next() % 10);
            cell.Add(rand.Next() % 10);
            return Ok(cell);
        }
    }
}
