using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface IGameRepository
    {
        Task<int> PostGameAsync(Game game);
        Task<Game> GetGameAsync(int gameId);
        Task<Game> UpdateGameAsync(Game game);
    }
}
