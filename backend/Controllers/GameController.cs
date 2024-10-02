using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Stream;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private static readonly StreamHandler _streamHandler = new StreamHandler("games.json");

        private static readonly List<Game> games = _streamHandler.ReadJsonObject<List<Game>>();


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
            _streamHandler.WriteJsonObject(games);

            return CreatedAtAction(nameof(GetGames), new { id = newGame.Id }, newGame);
        }
    }
}