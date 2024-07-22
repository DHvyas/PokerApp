namespace PokerApp.Server.Models.RequestModels
{
    public class StartGameRequest
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
    }
}
