using backend.DTOs.Leaderboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface ILeaderboardRepository
    {
        Task<IEnumerable<LeaderboardAccountDTO>> GetLeaderboard();
        Task<LeaderboardAccountDTO> GetAccountPlace(string username);
    }
}