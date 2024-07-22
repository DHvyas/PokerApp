namespace PokerApp.Server.Models
{
    public class FetchCardsResponse
    {
        public Hand Hand { get; set; }
        public CommunityCards CommunityCards { get; set; }
    }
}
