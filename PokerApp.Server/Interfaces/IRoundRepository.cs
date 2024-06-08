using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface IRoundRepository
    {
        Task<int> PostRound(Round round);
        Task<Round> GetRoundAsync(int roundId);
        Task<Round> UpdateRoundAsync(Round round);
        Task<Round> GetLatestRoundAsync(int gameId);
    }
}
