using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface IBetRepository
    {
        Task<int> PostBetAsync(Bet bet);
        Task<Bet> GetBetAsync(int gameId, int userId);
        Task<List<Bet>> GetBetsAsync(int gameId);
    }
}
