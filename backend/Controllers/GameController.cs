using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Interfaces;
using backend.Services;
using backend.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GamesController> _logger;
        private readonly Validator<Game> _gameValidator;

        public GamesController(IGameRepository gameRepository, ILogger<GamesController> logger, Validator<Game> gameValidator)
        {
            _gameRepository = gameRepository;
            _logger = logger;
            _gameValidator = gameValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _gameRepository.GetAllGames();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gameRepository.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<Game>> AddGame(Game game)
        {
            _gameValidator.Validate(game);
            await _gameRepository.AddGame(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            await _gameRepository.UpdateGame(game);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _gameRepository.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }

            await _gameRepository.DeleteGame(id);
            return NoContent();
        }
    }
}