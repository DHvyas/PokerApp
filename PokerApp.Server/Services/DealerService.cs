using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;

namespace PokerApp.Server.Services
{
    public class DealerService : IDealerService
    {
        private Deck deck;
        public DealerService()
        {
            deck = new Deck();
        }
        public void ShuffleDeck()
        {
            deck.Shuffle();
        }
        public Card DealCard()
        {
            var card = deck.DrawCard();
            return card;
        }
        public List<Card> DealCards(int numberOfCards)
        {
            var cards = new List<Card>();
            for (int i = 0; i < numberOfCards; i++)
            {
                cards.Add(deck.DrawCard());
            }
            return cards;
        }
    }
}
