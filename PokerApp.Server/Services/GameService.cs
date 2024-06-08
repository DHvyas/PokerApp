using Microsoft.EntityFrameworkCore;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;

namespace PokerApp.Server.Services
{
    public class GameService : IGameService
    {
        private readonly IHandService _handService;
        private readonly IGameRepository _gameRepository;
        private readonly IGamePlayerRepository _gamePlayerRepository;
        private readonly IRoundService _roundService;
        public GameService(IHandService handService, IGameRepository gameRepository, IGamePlayerRepository gamePlayerRepository, IRoundService roundService)
        {
            _handService = handService;
            _gameRepository = gameRepository;
            _gamePlayerRepository = gamePlayerRepository;
            _roundService = roundService;
        }
        public async Task<Game> CreateGameAsync(string gameName)
        {
            var game = new Game
            {
                GameName = gameName,
                Status = "Created",
                CreatedDate = DateTime.UtcNow,
                PotAmount = 0
            };
            try
            {
                game.GameID = await _gameRepository.PostGameAsync(game);
            }
            catch (Exception)
            {
                //Implement logging
                return null;
            }

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
            try
            {
                var game = await _gameRepository.GetGameAsync(gameId);
                return game;
            }
            catch (Exception)
            {
                //Implement logging
                return null;
            }
        }

        public async Task<bool> JoinGameAsync(int gameId, int userId, decimal initialChips)
        {
            try
            {
                var game = await GetGameAsync(gameId);
                if (game != null)
                {
                    var gamePlayer = new GamePlayer
                    {
                        GameID = gameId,
                        UserID = userId,
                        IsActive = true,
                        Chips = initialChips
                    };
                    gamePlayer.GamePlayerID = await _gamePlayerRepository.PostGamePlayerAsync(gamePlayer);
                    return true;
                }
                else
                {
                    throw new Exception("Game not found");
                }

            }
            catch (Exception)
            {
                //Implement logging
                return false;
            }
        }

        public async Task<bool> PerformActionAsync(int gameId, int userId, string actionType, decimal amount)
        {
            try
            {
                var game = await GetGameAsync(gameId);
                if (game == null)
                    throw new Exception("Game not found");
                if(game.Status != "Started")
                    throw new Exception("Game not started");
                var gamePlayer = await _gamePlayerRepository.GetGamePlayerAsync(gameId, userId);
                if (gamePlayer == null)
                    throw new Exception("User not found in game");
                if (game.CurrentTurnUserID != userId)
                    throw new Exception("Not your turn");
                if (actionType == "Fold")
                    {
                    gamePlayer.IsActive = false;
                    await _gamePlayerRepository.UpdateGamePlayer(gamePlayer);
                }
                else if (actionType == "Bet")
                {
                    if (amount > gamePlayer.Chips)
                        throw new Exception("Insufficient chips");
                    gamePlayer.Chips -= amount;
                    game.PotAmount += amount;
                    await _gamePlayerRepository.UpdateGamePlayer(gamePlayer);
                    await _gameRepository.UpdateGameAsync(game);
                }
                else if (actionType == "Call")
                {
                    if (amount > gamePlayer.Chips)
                        throw new Exception("Insufficient chips");
                    gamePlayer.Chips -= amount;
                    game.PotAmount += amount;
                    await _gamePlayerRepository.UpdateGamePlayer(gamePlayer);
                    await _gameRepository.UpdateGameAsync(game);
                }
                else if (actionType == "Check")
                {
                    //Do nothing
                }
                else
                {
                    throw new Exception("Invalid action");
                }
            }
            catch (Exception)
            {
                // Implement logging
                return false;
            }
            return true;
        }

        public async Task<bool> StartGameAsync(int gameId, int userId)
        {
            try
            {
                var game = await GetGameAsync(gameId);
                if (game != null)
                {
                    if(game.Status != "Created")
                        throw new Exception("Game already started");
                    game.Status = "Started";
                    var gamePlayer = await _gamePlayerRepository.GetGamePlayerAsync(gameId, userId);
                    if (gamePlayer == null)
                        throw new Exception("User not found in game");
                    game.CurrentTurnUserID = userId;
                    await _gameRepository.UpdateGameAsync(game);
                    _roundService.StartRoundAsync(gameId, 1);
                    var listOfGamePlayers = await _gamePlayerRepository.GetAllGamePlayersAsync(gameId);
                    foreach (var player in listOfGamePlayers)
                    {
                        _handService.DealHandAsync(player.GamePlayerID);
                    }
                    game.CurrentTurnUserID = listOfGamePlayers[0].UserID;
                    await _gameRepository.UpdateGameAsync(game);
                    return true;
                }
                else
                {
                    throw new Exception("Game not found");
                }
            }
            catch (Exception)
            {
                //Implement logging
                return false;
            }
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
