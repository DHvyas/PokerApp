using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;

namespace PokerApp.Server.Services
{
    public class RoundService : IRoundService
    {
        private readonly IRoundRepository _roundRepository;
        public RoundService(IRoundRepository roundRepository)
        {
            _roundRepository = roundRepository;
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
            var round = new Round
            {
                GameID = gameId,
                RoundNumber = roundNumber,
                RoundName = ((RoundName)roundNumber).ToString(),
                StartTime = DateTime.UtcNow,
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
    }
}
