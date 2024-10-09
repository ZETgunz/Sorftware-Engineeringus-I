using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SequenceController : ControllerBase
    {
        private static readonly Random rand = new Random();

        [HttpGet("{level}")]
        public ActionResult<List<SequenceCell>> GetCell(int level)
        {
            List<SequenceCell> sequence = new List<SequenceCell>{};
            while(sequence.Count()!=level){
                sequence.Add(new SequenceCell {row = rand.Next() % 3, column = rand.Next() % 3 });
            }
            return Ok(sequence);
        }
    }

}