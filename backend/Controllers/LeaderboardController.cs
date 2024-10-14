using backend.Models;
using backend.DTOs.Account;
using backend.DTOs.Leaderboard;
using Microsoft.AspNetCore.Mvc;
using backend.Mappers;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private static readonly JsonCRUD _JsonCRUD = new JsonCRUD("accounts.json");
        private static List<Account> accounts = new List<Account>();

        private static List<LeaderboardAccountDTO> leaderboard = new List<LeaderboardAccountDTO>();

        [HttpGet]
        public ActionResult<IEnumerable<LeaderboardAccountDTO>> GetLeaderboard()
        {
            accounts = _JsonCRUD.ReadJsonObject<List<Account>>();
            accounts.Sort();
            leaderboard = accounts.Select(account => account.AccountToLeaderboardAccountDTO(accounts.IndexOf(account) + 1)).ToList();
            return Ok(leaderboard);
        }

        [HttpGet("{username}")]
        public ActionResult<AccountDTO> GetAccountPlace([FromRoute] string username)
        {
            accounts = _JsonCRUD.ReadJsonObject<List<Account>>();
            accounts.Sort();
            Account account = accounts.Find(account => account.Username == username);
            if (account == null)
            {
                return NotFound("Account not found with username :" + username);
            }
            return Ok(account.AccountToLeaderboardAccountDTO(accounts.IndexOf(account) + 1));

        }
    }
}