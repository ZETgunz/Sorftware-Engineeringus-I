using backend.Models;
using backend.DTOs.Account;
using backend.DTOs.Leaderboard;
using Microsoft.AspNetCore.Mvc;
using backend.Mappers;
using backend.Services;
using backend.Data;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private static List<Account> accounts = new List<Account>();

        private static List<LeaderboardAccountDTO> leaderboard = new List<LeaderboardAccountDTO>();

        [HttpGet]
        public ActionResult<IEnumerable<LeaderboardAccountDTO>> GetLeaderboard()
        {
            accounts.Clear();
            foreach (AccountDTO accountDTO in _context.AccountDTOs.ToList())
            {
                accounts.Add(accountDTO.AccountDTOToAccount());
            }
            accounts.Sort();
            leaderboard = accounts.Select(account => account.AccountToLeaderboardAccountDTO(accounts.IndexOf(account) + 1)).ToList();
            return Ok(leaderboard);
        }

        [HttpGet("{username}")]
        public ActionResult<AccountDTO> GetAccountPlace([FromRoute] string username)
        {
            accounts.Clear();
            foreach (AccountDTO accountDTO in _context.AccountDTOs.ToList())
            {
                accounts.Add(accountDTO.AccountDTOToAccount());
            }
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