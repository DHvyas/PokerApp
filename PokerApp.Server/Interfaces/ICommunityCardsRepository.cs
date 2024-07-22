using PokerApp.Server.Models;

namespace PokerApp.Server.Interfaces
{
    public interface ICommunityCardsRepository
    {
        Task<int> PostCommunityCardsAsync(CommunityCards communityCards);
        Task<CommunityCards> GetCommunityCardsAsync(int gameId);
        Task<bool> UpdateCommunityCardsAsync(CommunityCards communityCards);

    }
}
