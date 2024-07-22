namespace PokerApp.Server.Models.RequestModels
{
    public class JoinGameRequest
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public decimal InitialChips { get; set; }
    }
}
