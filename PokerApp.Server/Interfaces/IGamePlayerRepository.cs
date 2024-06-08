using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface IGamePlayerRepository
    {
        Task<GamePlayer> UpdateGamePlayer(GamePlayer gamePlayer);
        Task<GamePlayer> GetGamePlayerAsync(int gamePlayerId);
        Task<int> PostGamePlayerAsync(GamePlayer gamePlayer);
        Task<GamePlayer> GetGamePlayerAsync(int gameID, int UserID);
        Task<List<GamePlayer>> GetAllGamePlayersAsync(int gameID);
    }
}
