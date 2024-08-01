using PokerApp.Server.Models;
using System.Collections.Generic;

namespace PokerApp.Server.Interfaces
{
    public interface IDealerService
    {
        void ShuffleDeck(int gameId);
        Card DealCard(int gameId);
        List<Card> DealCards(int gameId, int numberOfCards);
    }
}
