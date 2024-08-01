using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;

namespace PokerApp.Server.Services
{
    public class RoundService : IRoundService
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IDealerService _dealerService;
        private readonly ICommunityCardsRepository _communityCardsRepository;
        public RoundService(IRoundRepository roundRepository, IDealerService dealerService, ICommunityCardsRepository communityCardsRepository)
        {
            _roundRepository = roundRepository;
            _dealerService = dealerService;
            _communityCardsRepository = communityCardsRepository;
        }
        public async Task<bool> EndRoundAsync(int roundId)
        {
            try
            {
                var round = await _roundRepository.GetRoundAsync(roundId);
                if(round == null)
                {
                    throw new Exception("Round not found");
                }
                round.EndTime = DateTime.UtcNow;
                round = await _roundRepository.UpdateRoundAsync(round);
            }
            catch (Exception)
            {
                //Implement logging
                return false;
            }
            return true;
        }

        public async void StartRoundAsync(int gameId, int roundNumber)
        {
            var communityCards = new CommunityCards
            {
                GameID = gameId,
                Card1 = null,
                Card2 = null,
                Card3 = null,
                Card4 = null,
                Card5 = null
            };
            _dealerService.ShuffleDeck(gameId);
            //To handle Flop
            if(roundNumber == 2)
            {
                var cards = _dealerService.DealCards(gameId, 3);
                communityCards.Card1 = cards[0].ToString();
                communityCards.Card2 = cards[1].ToString();
                communityCards.Card3 = cards[2].ToString();
                await _communityCardsRepository.PostCommunityCardsAsync(communityCards);
            }
            if(roundNumber > 2)
            {
                communityCards = await _communityCardsRepository.GetCommunityCardsAsync(gameId);
                if(roundNumber == 4)
                {
                    communityCards.Card5 = _dealerService.DealCard(gameId).ToString();
                }
                else
                {
                    communityCards.Card4 = _dealerService.DealCard(gameId).ToString();
                }
                await _communityCardsRepository.UpdateCommunityCardsAsync(communityCards);
            }
            var round = new Round
            {
                GameID = gameId,
                RoundNumber = roundNumber,
                RoundName = ((RoundName)roundNumber).ToString(),
                StartTime = DateTime.UtcNow,
                RoundAmount = 0,
                EndTime = null
            };
            try
            {
                round.RoundID = await _roundRepository.PostRound(round);
            }
            catch (Exception)
            {
                //Implement logging
            }
        }
        public async Task<Round> GetRoundAsync(int roundId)
        {
            return await _roundRepository.GetRoundAsync(roundId);
        }
        public async Task<Round> GetLatestRoundAsync(int gameId)
        {
            return await _roundRepository.GetLatestRoundAsync(gameId);
        }
    }
}
