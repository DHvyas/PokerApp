namespace PokerApp.Server.Models
{
    public class GameSession
    {
        public Deck Deck { get; private set; }

        public GameSession()
        {
            Deck = new Deck();
        }

        public void ShuffleDeck()
        {
            Deck.Shuffle();
        }

        public Card DealCard()
        {
            return Deck.DrawCard();
        }

        public List<Card> DealCards(int numberOfCards)
        {
            var cards = new List<Card>();
            for (int i = 0; i < numberOfCards; i++)
            {
                cards.Add(Deck.DrawCard());
            }
            return cards;
        }
    }
}
