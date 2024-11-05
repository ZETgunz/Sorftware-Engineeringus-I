using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Data;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        private static List<Game> games = new List<Game>();

        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetGames()
        {
            games = _context.Games.ToList();
            return Ok(games);
        }

    }
}