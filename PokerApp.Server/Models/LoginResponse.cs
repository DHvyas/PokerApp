namespace PokerApp.Server.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
