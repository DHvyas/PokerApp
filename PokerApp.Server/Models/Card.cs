namespace PokerApp.Server.Models
{
    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }
        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }
        public Card(string cardString)
        {
            if (cardString.Length != 2)
                cardString = ("T" + cardString[cardString.Length - 1]);
            Rank = cardString[0] switch
            {
                '2' => Rank.Two,
                '3' => Rank.Three,
                '4' => Rank.Four,
                '5' => Rank.Five,
                '6' => Rank.Six,
                '7' => Rank.Seven,
                '8' => Rank.Eight,
                '9' => Rank.Nine,
                'T' => Rank.Ten,
                'J' => Rank.Jack,
                'Q' => Rank.Queen,
                'K' => Rank.King,
                'A' => Rank.Ace,
                _ => throw new ArgumentException("Invalid Rank")
            };
            Suit = cardString[1] switch
            {
                'H' => Suit.Hearts,
                'D' => Suit.Diamond,
                'C' => Suit.Club,
                'S' => Suit.Spade,
                _ => throw new ArgumentException("Invalid Suit")
            };
        }
        public override string ToString()
        {
            string rankString = Rank switch
            {
                Rank.Two => "2",
                Rank.Three => "3",
                Rank.Four => "4",
                Rank.Five => "5",
                Rank.Six => "6",
                Rank.Seven => "7",
                Rank.Eight => "8",
                Rank.Nine => "9",
                Rank.Ten => "10",
                Rank.Jack => "J",
                Rank.Queen => "Q",
                Rank.King => "K",
                Rank.Ace => "A",
                _ => "Invalid Rank"
            };
            string suitString = Suit switch
            {
                Suit.Hearts => "H",
                Suit.Diamond => "D",
                Suit.Club => "C",
                Suit.Spade => "S",
                _ => "Invalid Suit"
            };
            return rankString + suitString;
        }
    }
    public enum Suit
    {
        Hearts,
        Diamond,
        Club,
        Spade
    }
    public enum Rank
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }
}
