namespace PokerApp.Server.Models
{
    public class GetAllGamesResponse
    {
        public List<Game> AllGames { get; set; }
        public List<Game> GamesJoined { get; set; }
    }
}
