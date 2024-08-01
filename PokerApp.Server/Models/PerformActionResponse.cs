namespace PokerApp.Server.Models
{
    public class PerformActionResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsGameOver { get; set; }
    }
}
