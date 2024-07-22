using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface IUserRepository
    {
        Task<int> PostUserAsync(User user);
        Task<User> GetUserAsync(string userName);
        Task<User> GetUserAsync(int userId);
    }
}
