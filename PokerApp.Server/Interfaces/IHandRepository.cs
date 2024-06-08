using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface IHandRepository
    {
        Task<int> PostHandAsync(Hand hand);
    }
}
