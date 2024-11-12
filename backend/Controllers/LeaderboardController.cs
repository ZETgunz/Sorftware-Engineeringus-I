using backend.DTOs.Account;
using backend.DTOs.Leaderboard;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardRepository _leaderboardRepository;

        public LeaderboardController(ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardAccountDTO>>> GetLeaderboard()
        {
            var leaderboard = await _leaderboardRepository.GetLeaderboard();
            return Ok(leaderboard);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<LeaderboardAccountDTO>> GetAccountPlace([FromRoute] string username)
        {
            try
            {
                var accountPlace = await _leaderboardRepository.GetAccountPlace(username);
                return Ok(accountPlace);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
