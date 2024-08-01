using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using PokerApp.Server.Models.RequestModels;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokerApp.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        readonly IGameService _gameService;
        readonly IRoundService _roundService;
        public GameController(IGameService gameService, IRoundService roundService)
        {
            _gameService = gameService;
            _roundService = roundService;
        }
        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllGames([FromQuery] int userId)
        {
            var games = await _gameService.GetAllGamesAsync();
            var gamesJoined = await _gameService.GetGamesJoinedAsync(userId);
            games.RemoveAll(g => gamesJoined.Exists(gj => gj.GameID == g.GameID));
            GetAllGamesResponse response = new GetAllGamesResponse();
            response.AllGames = games;
            response.GamesJoined = gamesJoined;
            if (games == null)
                return NotFound();
            return Ok(response);
        }
        //Get Game
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetGame([FromQuery] int gameId)
        {
            var game = await _gameService.GetGameAsync(gameId);
            var round = await _roundService.GetLatestRoundAsync(gameId);
            var users = await _gameService.GetAllGamePlayerUsersAsync(gameId);
            GetGameResponse response = new GetGameResponse();
            response.game = game;
            response.round = round;
            response.users = users;
            if (game == null)
                return NotFound();
            return Ok(response);
        }
        //Create Game
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest createGameRequest)
        {
            var game = await _gameService.CreateGameAsync(createGameRequest.GameName);
            if (game == null)
                return NotFound();
            return Ok(game);
        }
        //Join Game
        [HttpPost]
        [Route("join")]
        public async Task<IActionResult> JoinGame([FromBody] JoinGameRequest joinGameRequest)
        {
            var game = await _gameService.JoinGameAsync(joinGameRequest.GameId, joinGameRequest.UserId, joinGameRequest.InitialChips);
            if (game == null)
                return NotFound();
            return Ok(game);
        }
        //Start Game
        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> StartGame([FromBody] StartGameRequest startGameRequest)
        {
            return await _gameService.StartGameAsync(startGameRequest.GameId, startGameRequest.UserId) ? Ok() : NotFound();
        }
        //Fetch Cards
        [HttpGet]
        [Route("fetchCards")]
        public async Task<IActionResult> FetchCardsAsync([FromQuery] int gameId, int userId)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var response = await _gameService.FetchCardsAsync(gameId, userId, userName);
            return response == null ? NotFound() : Ok(response);
        }  
        //Perform Action
        [HttpPost]
        [Route("performAction")]
        public async Task<IActionResult> PerformAction([FromBody] PerformActionRequest performActionRequest)
        {
            var response = await _gameService.PerformActionAsync(performActionRequest.GameId, performActionRequest.UserId, performActionRequest.ActionType, performActionRequest.Amount);
            return response == null ? NotFound() : Ok(response);
        }
    }
}
