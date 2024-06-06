using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;

namespace PokerApp.Server.Services
{
    public class HandService : IHandService
    {
        public Task<Hand> DealHandAsync(int gamePlayerId, string card1, string card2)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteHandAsync(int handId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateHandAsync(int handId, string newCard1, string newCard2)
        {
            throw new NotImplementedException();
        }
    }
}
