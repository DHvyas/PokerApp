namespace PokerApp.Server.Interfaces;
using System.Threading.Tasks;
using PokerApp.Server.Models;

public interface IBetService
{
    Task<Bet> PlaceBetAsync(int roundId, int gamePlayerId, decimal betAmount);
    Task<bool> UpdateBetAsync(int betId, decimal newBetAmount);
    Task<bool> DeleteBetAsync(int betId);
}

