using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.Collections.Concurrent;

namespace PokerApp.Server.Services
{
    public class DealerService : IDealerService
    {
        private readonly ConcurrentDictionary<string, GameSession> _gameSessions;

        public DealerService()
        {
            _gameSessions = new ConcurrentDictionary<string, GameSession>();
        }

        public void ShuffleDeck(int gameId)
        {
            var gameSession = GetOrCreateGameSession(gameId);
            gameSession.ShuffleDeck();
        }

        public Card DealCard(int gameId)
        {
            var gameSession = GetOrCreateGameSession(gameId);
            return gameSession.DealCard();
        }

        public List<Card> DealCards(int gameId, int numberOfCards)
        {
            var gameSession = GetOrCreateGameSession(gameId);
            return gameSession.DealCards(numberOfCards);
        }

        private GameSession GetOrCreateGameSession(int gameId)
        {
            return _gameSessions.GetOrAdd(gameId.ToString(), id => new GameSession());
        }
    }
}
