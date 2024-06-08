namespace PokerApp.Server.Interfaces;
using System.Threading.Tasks;
using PokerApp.Server.Models;

public interface IRoundService
{
    void StartRoundAsync(int gameId, int roundNumber);
    Task<bool> EndRoundAsync(int roundId);
}

