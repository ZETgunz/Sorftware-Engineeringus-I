using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllGames();
        Task<Game> GetGameById(int id);
        Task AddGame(Game game);
        Task UpdateGame(Game game);
        Task DeleteGame(int id);
    }
}