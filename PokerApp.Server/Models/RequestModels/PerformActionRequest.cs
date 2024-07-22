namespace PokerApp.Server.Models.RequestModels
{
    public class PerformActionRequest
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string ActionType { get; set; }
        public decimal Amount { get; set; }
    }
}
