namespace PokerApp.Server.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using PokerApp.Server.Models;

public interface IGameService
{
    Task<Game> CreateGameAsync(string gameName);
    Task<Game> GetGameAsync(int gameId);
    Task<List<Game>> GetAllGamesAsync();
    Task<bool> UpdateGameAsync(int gameId, Game updatedGame);
    Task<bool> DeleteGameAsync(int gameId);
    Task<bool> JoinGameAsync(int gameId, int userId, decimal initialChips);
    Task<bool> StartGameAsync(int gameId, int userId);
    Task<PerformActionResponse> PerformActionAsync(int gameId, int userId, string actionType, decimal amount);
    Task<bool> EndRoundAsync(int gameId);
    Task<FetchCardsResponse> FetchCardsAsync(int gameId, int userId, string userName);
    Task<List<Game>> GetGamesJoinedAsync(int userId);
    Task<List<User>> GetAllGamePlayerUsersAsync(int gameId);
}

