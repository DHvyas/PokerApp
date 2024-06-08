namespace PokerApp.Server.Models
{
    public class Deck
    {
        private List<Card> cards;
        private Random rng;
        public Deck()
        {
            cards = new List<Card>();
            rng = new Random();
            InitializeDeck();
        }
        private void InitializeDeck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }
        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 0)
            {
                n--;
                int k = rng.Next(n+1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }
        public Card DrawCard()
        {
            if (cards.Count == 0)
                throw new Exception("Deck is empty");
            Card drawnCard = cards[cards.Count-1];
            cards.RemoveAt(cards.Count-1);
            return drawnCard;
        }
    }
}
