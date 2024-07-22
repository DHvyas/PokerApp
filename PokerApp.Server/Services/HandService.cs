using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;

namespace PokerApp.Server.Services
{
    public class HandService : IHandService
    {
        private readonly IDealerService _dealerService;
        private readonly IHandRepository _handRepository;
        private Hand hand;
        public HandService(IDealerService dealerService, IHandRepository handRepository)
        {
            _dealerService = dealerService;
            _handRepository = handRepository;
        }
        public async void DealHandAsync(int gamePlayerId)
        {
            hand = new Hand();
            hand.GamePlayerID = gamePlayerId;
            hand.Card1 = _dealerService.DealCard().ToString();
            hand.Card2 = _dealerService.DealCard().ToString();
            hand.HandID = await _handRepository.PostHandAsync(hand);
        }
        public async Task<Hand> GetHandAsync(int handId)
        {
            return await _handRepository.GetHandAsync(handId);
        }
        public Task<bool> DeleteHandAsync(int handId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateHandAsync(int handId, string newCard1, string newCard2)
        {
            throw new NotImplementedException();
        }
    }
}
