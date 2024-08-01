namespace PokerApp.Server.Interfaces;
using System.Threading.Tasks;
using PokerApp.Server.Models;

public interface IHandService
{
    void DealHandAsync(int gamePlayerId, int gameId);
    Task<bool> UpdateHandAsync(int handId, string newCard1, string newCard2);
    Task<bool> DeleteHandAsync(int handId);
    Task<Hand> GetHandAsync(int handId);
}

