using Microsoft.EntityFrameworkCore;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;

namespace PokerApp.Server.Services
{
    public class GameService : IGameService
    {
        private readonly IHandService _handService;
        public GameService(IHandService handService)
        {
            _handService = handService;
        }
        public async Task<Game> CreateGameAsync(string gameName)
        {
            var game = new Game
            {
                GameName = gameName,
                Status = "Created"
            };

/*            _context.Games.Add(game);
            await _context.SaveChangesAsync();*/

            return game;
        }

        public Task<bool> DeleteGameAsync(int gameId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EndRoundAsync(int gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Game>> GetAllGamesAsync()
        {
            //return await _context.Games.ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<Game> GetGameAsync(int gameId)
        {
            //return await _context.Games.FindAsync(gameId);
            throw new NotImplementedException();
        }

        public Task<bool> JoinGameAsync(int gameId, int userId, decimal initialChips)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PerformActionAsync(int gameId, int userId, string actionType, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StartGameAsync(int gameId)
        {
            /*var game = await _context.Games.FindAsync(gameId);
            if (game == null)
                return false;
            game.Status = "Started";
            return true;*/
           // _context.GamePlayers.FindAsync(gp => gp.GameID == gameId).ForEach(gp => _handService.DealHandAsync( gp.GamePlayerId));
           throw new NotImplementedException();
        }

        public async Task<bool> UpdateGameAsync(int gameId, Game updatedGame)
        {
            /*var game = await _context.Games.FindAsync(gameId);
            if (game == null)
                return false;

            game.GameName = updatedGame.GameName;
            game.Status = updatedGame.Status;

            await _context.SaveChangesAsync();
            return true;*/
            throw new NotImplementedException();
        }
    }
}
