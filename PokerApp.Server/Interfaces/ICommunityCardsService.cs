namespace PokerApp.Server.Interfaces;
using System.Threading.Tasks;
using PokerApp.Server.Models;

public interface ICommunityCardsService
{
    Task<CommunityCards> DealCommunityCardsAsync(int gameId, string card1, string card2, string card3, string card4, string card5);
    Task<bool> UpdateCommunityCardsAsync(int communityCardsId, string newCard1, string newCard2, string newCard3, string newCard4, string newCard5);
    Task<bool> DeleteCommunityCardsAsync(int communityCardsId);
}