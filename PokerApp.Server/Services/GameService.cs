using Microsoft.EntityFrameworkCore;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using PokerApp.Server.Repositories;
using System.Numerics;

namespace PokerApp.Server.Services
{
    public class GameService : IGameService
    {
        private readonly IHandService _handService;
        private readonly IUserService _userService;
        private readonly IGameRepository _gameRepository;
        private readonly IGamePlayerRepository _gamePlayerRepository;
        private readonly IRoundService _roundService;
        private readonly IBetRepository _betRepository;
        private readonly ICommunityCardsRepository _communityCardsRepository;
        public GameService(IHandService handService, IGameRepository gameRepository, IGamePlayerRepository gamePlayerRepository, IRoundService roundService, IUserService userService, IBetRepository betRepository, ICommunityCardsRepository communityCardsRepository)
        {
            _handService = handService;
            _gameRepository = gameRepository;
            _gamePlayerRepository = gamePlayerRepository;
            _roundService = roundService;
            _userService = userService;
            _betRepository = betRepository;
            _communityCardsRepository = communityCardsRepository;
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
            try
            {
                var games = await _gameRepository.GetAllGamesAsync();
                return games;
            }
            catch (Exception)
            {
                //Implement logging
                return null;
            }
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
        public async Task<List<Game>> GetGamesJoinedAsync(int userId)
        {
            try
            {
                var games = await _gameRepository.GetGamesJoinedAsync(userId);
                return games;
            }
            catch (Exception)
            {
                return new List<Game>();
            }
        }

        public async Task<bool> JoinGameAsync(int gameId, int userId, decimal initialChips)
        {
            try
            {
                if (!await _userService.IsUserExistsAsync(userId))
                    throw new Exception("User not found");
                var game = await GetGameAsync(gameId);
                if (game != null)
                {
                    if ((await _gamePlayerRepository.GetGamePlayerAsync(gameId, userId))?.GamePlayerID > 0)
                        throw new Exception("Player already in game");
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
                var round = await _roundService.GetLatestRoundAsync(gameId);
                var bet = await _betRepository.GetBetAsync(gameId, userId);
                if (gamePlayer == null)
                    throw new Exception("User not found in game");
                if (game.CurrentTurnUserID != userId)
                    throw new Exception("Not your turn");
                bet ??= new Bet
                    {
                        GamePlayerID = gamePlayer.GamePlayerID,
                        BetAmount = 0,
                        BetTime = DateTime.UtcNow,
                        RoundID = round.RoundID
                    };
                if (actionType == "Fold")
                {
                    gamePlayer.IsActive = false;
                }
                else if (actionType == "Bet")
                {
                    if (amount > gamePlayer.Chips)
                        throw new Exception("Insufficient chips");
                    if (amount < round.RoundAmount)
                        throw new Exception("Bet amount less than round amount");
                    gamePlayer.Chips -= amount;
                    game.PotAmount += amount;
                    round.RoundAmount = amount;
                    bet.BetAmount += amount;
                }
                else if (actionType == "Call")
                {
                    if (amount > gamePlayer.Chips)
                        throw new Exception("Insufficient chips");
                    if (amount != round.RoundAmount)
                        throw new Exception("Call amount less than round amount");
                    gamePlayer.Chips -= amount;
                    game.PotAmount += amount;
                    bet.BetAmount += amount;
                }
                else if (actionType == "Check")
                {
                    //Do nothing
                }
                else
                {
                    throw new Exception("Invalid action");
                }

                await _gamePlayerRepository.UpdateGamePlayer(gamePlayer);
                await _betRepository.PostBetAsync(bet);
                var listOfGamePlayers = await _gamePlayerRepository.GetAllGamePlayersAsync(gameId);
                if(await IsRoundComplete(gameId))
                {
                    _roundService.EndRoundAsync(round.RoundID);
                    _roundService.StartRoundAsync(gameId, round.RoundNumber + 1);
                    game.CurrentTurnUserID = listOfGamePlayers.Find(x => x.IsActive).UserID;
                }
                else
                {
                    var nextPlayerIndex = (listOfGamePlayers.FindIndex(x => x.UserID == game.CurrentTurnUserID) + 1) % listOfGamePlayers.Count;
                    game.CurrentTurnUserID = listOfGamePlayers[nextPlayerIndex].UserID;
                }
                if(round.RoundNumber.Equals(4))
                {
                    await EndGameAsync(gameId);
                    game.Equals("Ended");
                }
                await _gameRepository.UpdateGameAsync(game);
            }
            catch (Exception)
            {
                // Implement logging
                return false;
            }
            return true;
        }
        public async Task<List<User>> GetAllGamePlayerUsersAsync(int gameId)
        {
            try
            {
                var gamePlayers = await _gamePlayerRepository.GetAllGamePlayersAsync(gameId);
                var users = new List<User>();
                foreach (var gamePlayer in gamePlayers)
                {
                    var user = await _userService.GetUserAsync(gamePlayer.UserID);
                    users.Add(user);
                }
                users.ForEach(user =>
                {
                    user.PasswordHash = null;
                    user.TotalWinnings = 0;

                });
                return users;
            }
            catch (Exception)
            {
                //Implement logging
                return null;
            }
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
        public async Task<FetchCardsResponse> FetchCardsAsync(int gameId, int userId, string userName)
        {
            try
            {
                var user = await _userService.GetUserAsync(userName);
                if (user?.UserName != userName)
                    throw new Exception("Authentication Error");
                var gamePlayer = await _gamePlayerRepository.GetGamePlayerAsync(gameId, userId);
                if (gamePlayer == null)
                    throw new Exception("User not found in game");
                var hand = await _handService.GetHandAsync(gamePlayer.GamePlayerID);
                var communityCards = await _communityCardsRepository.GetCommunityCardsAsync(gameId);
                if (hand == null)
                {
                    _handService.DealHandAsync(gamePlayer.GamePlayerID);
                    hand = await _handService.GetHandAsync(gamePlayer.GamePlayerID);
                }

                hand.GamePlayer = gamePlayer;
                FetchCardsResponse response = new FetchCardsResponse
                {
                    Hand = hand,
                    CommunityCards = communityCards
                };
                return response;
            }
            catch (Exception)
            {
                //Implement logging
                return null;
            }
        }
        private async Task<bool> IsRoundComplete(int gameId)
        {
            try
            {
                var bets = await _betRepository.GetBetsAsync(gameId);
                var game = await GetGameAsync(gameId);
                var highestBet = bets?.Max(x => x.BetAmount) ?? 0;
                var players = await _gamePlayerRepository.GetAllGamePlayersAsync(gameId);

                if (players.Count <= 1)
                {
                    return true;
                }

                foreach (var player in players)
                {
                    var playerBet = bets?.FirstOrDefault(x => x.GamePlayerID == player.GamePlayerID);

                    // Assuming a player who hasn't placed a bet or has folded is represented by playerBet being null or bet amount being 0
                    if (playerBet == null || playerBet.BetAmount < highestBet)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> EndGameAsync(int gameId)
        {
            try
            {
                var game = await GetGameAsync(gameId);
                if (game == null)
                    throw new Exception("Game not found");
                var players = await _gamePlayerRepository.GetAllGamePlayersAsync(gameId);
                var communityCards = await _communityCardsRepository.GetCommunityCardsAsync(gameId);
                foreach (var player in players)
                {
                    var hand = await _handService.GetHandAsync(player.GamePlayerID);
                    player.HandValue = EvaluateHand(hand, communityCards);
                    player.IsActive = false;
                    await _gamePlayerRepository.UpdateGamePlayer(player);
                }
                var winner = players.OrderByDescending(x => x.HandValue).First();
                game.WinnerUserID = winner.UserID;
                var winnerUser = await _userService.GetUserAsync(winner.UserID);
                winnerUser.TotalWinnings += game.PotAmount;
                //await _userService.(winnerUser.UserID, winnerUser);
                game.Status = "Ended";
                await _gameRepository.UpdateGameAsync(game);
            }
            catch (Exception)
            {
                //Implement logging
                return false;
            }
            return true;
        }
        private int EvaluateHand(Hand hand, CommunityCards communityCards)
        {
            int handValue = 0;
            List<Card> cards = new List<Card>();
            cards.Add(new Card(hand.Card1));
            cards.Add(new Card(hand.Card2));
            cards.Add(new Card(communityCards.Card1));
            cards.Add(new Card(communityCards.Card2));
            cards.Add(new Card(communityCards.Card3));
            cards.Add(new Card(communityCards.Card4));
            cards.Add(new Card(communityCards.Card5));
            cards = cards.OrderBy(card => card.Rank).ToList();
            if(IsRoyalFlush(cards))
            {
                handValue = 10;
            }
            else if(IsStraightFlush(cards))
            {
                handValue = 9;
            }
            else if(IsFourOfAKind(cards))
            {
                handValue = 8;
            }
            else if(IsFullHouse(cards))
            {
                handValue = 7;
            }
            else if(IsFlush(cards))
            {
                handValue = 6;
            }
            else if(IsStraight(cards))
            {
                handValue = 5;
            }
            else if(IsThreeOfAKind(cards))
            {
                handValue = 4;
            }
            else if(IsTwoPair(cards))
            {
                handValue = 3;
            }
            else if(IsOnePair(cards))
            {
                handValue = 2;
            }
            else
            {
                handValue = 1;
            }
            return handValue;

        }
        private bool IsRoyalFlush(List<Card> cards)
        {
            return IsStraightFlush(cards) && cards[4].Rank == Rank.Ace;
        }
        private bool IsStraightFlush(List<Card> cards)
        {
            return IsFlush(cards) && IsStraight(cards);
        }
        private bool IsFourOfAKind(List<Card> cards)
        {
            return HasNOfAKind(cards, 4);
        }
        private bool IsFullHouse(List<Card> cards)
        {
            return HasNOfAKind(cards, 3) && HasNOfAKind(cards, 2);
        }
        private bool IsFlush(List<Card> cards)
        {
            return cards.All(x => x.Suit == cards[0].Suit);
        }
        private bool IsStraight(List<Card> cards)
        {
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].Rank - cards[i + 1].Rank != 1)
                    return false;
            }
            return true;
        }
        private bool IsThreeOfAKind(List<Card> cards)
        {
            return HasNOfAKind(cards, 3);
        }
        private bool IsTwoPair(List<Card> cards)
        {
            return cards.GroupBy(x => x.Rank).Count(x => x.Count() == 2) == 2;
        }
        private bool IsOnePair(List<Card> cards)
        {
            return HasNOfAKind(cards, 2);
        }
        private bool HasNOfAKind(List<Card> cards, int n)
        {
            return cards.GroupBy(x => x.Rank).Any(x => x.Count() == n);
        }
    }
}
