namespace PokerApp.Server.Models
{
    public class GetGameResponse
    {
        public Game game { get; set; }
        public Round round { get; set; }
        public List<User> users { get; set; }
    }
}
