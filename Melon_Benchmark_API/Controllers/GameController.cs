using Melon_Benchmark_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("games")]
    public class GamesController : ControllerBase
    {
        private static readonly List<Game> games =
        [
            new Game { Id = 1, Name = "Melon Ninja", Description = "Slice as many Melons as possible", Route = "/melonNinja" },
            new Game { Id = 2, Name = "Sequence Memorization", Description = "Remeber the melon sequance", Route = "/sequance" },
            new Game { Id = 3, Name = "Speed Writing", Description = "Write the text as fast as possible the least amount of mistakes", Route = "/speedWriting"}
        ];


        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetGames()
        {
            return Ok(games);
        }

        [HttpPost]
        public IActionResult AddGame([FromBody] Game newGame)
        {
            if (newGame == null || string.IsNullOrWhiteSpace(newGame.Name))
            {
                return BadRequest("Game details cannot be empty.");
            }

            games.Add(newGame);

            return CreatedAtAction(nameof(GetGames), new { id = newGame.Id }, newGame);
        }
    }
}