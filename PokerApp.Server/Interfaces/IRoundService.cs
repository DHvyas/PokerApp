namespace PokerApp.Server.Interfaces;
using System.Threading.Tasks;
using PokerApp.Server.Models;

public interface IRoundService
{
    Task<Round> StartRoundAsync(int gameId, string roundName);
    Task<bool> EndRoundAsync(int roundId);
}

