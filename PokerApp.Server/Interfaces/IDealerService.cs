using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface IDealerService
    {
        public void ShuffleDeck();
        public Card DealCard();
        public List<Card> DealCards(int numberOfCards);
    }
}
