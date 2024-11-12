using backend.DTOs.Leaderboard;
using backend.Interfaces;
using backend.Data;
using backend.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly AppDbContext _context;

        public LeaderboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaderboardAccountDTO>> GetLeaderboard()
        {
            var accounts = await _context.AccountDTOs.ToListAsync();
            var accountList = accounts.Select(accountDTO => accountDTO.AccountDTOToAccount()).ToList();
            accountList.Sort();
            var leaderboard = accountList.Select((account, index) => account.AccountToLeaderboardAccountDTO(index + 1)).ToList();
            return leaderboard;
        }

        public async Task<LeaderboardAccountDTO> GetAccountPlace(string username)
        {
            var accounts = await _context.AccountDTOs.ToListAsync();
            var accountList = accounts.Select(accountDTO => accountDTO.AccountDTOToAccount()).ToList();
            accountList.Sort();
            var account = accountList.FirstOrDefault(a => a.Username == username);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found with username: " + username);
            }
            return account.AccountToLeaderboardAccountDTO(accountList.IndexOf(account) + 1);
        }
    }
}