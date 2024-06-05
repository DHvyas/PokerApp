namespace PokerApp.Server.Interfaces;
using System.Threading.Tasks;
using PokerApp.Server.Models;

public interface IHandService
{
    Task<Hand> DealHandAsync(int gamePlayerId, string card1, string card2);
    Task<bool> UpdateHandAsync(int handId, string newCard1, string newCard2);
    Task<bool> DeleteHandAsync(int handId);
}

