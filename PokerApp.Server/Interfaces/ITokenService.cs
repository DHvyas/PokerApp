using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
