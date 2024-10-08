using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.JsonCRUD;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private static readonly JsonCRUD _JsonCRUD = new JsonCRUD("games.json");

        private static List<Game> games = new List<Game>();


        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetGames()
        {
            games = _JsonCRUD.ReadJsonObject<List<Game>>();
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
            _JsonCRUD.WriteJsonObject(games);

            return CreatedAtAction(nameof(GetGames), new { id = newGame.Id }, newGame);
        }
    }
}