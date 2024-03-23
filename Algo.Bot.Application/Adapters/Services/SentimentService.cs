using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Entity;
using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.Ports;
using Algo.Bot.PositiveSentiment.Models.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Algo.Bot.Application.Adapters.Services
{
    public class SentimentService : ISentimentService
    {
        private readonly ICoinStorage _coinStorage;
        private readonly IOrderStorage _orderStorage;
        private readonly IOrderIndicator _buyIndicator;
        private readonly IMaintenanceIndicator _sentimentIndicator;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ILogger<SentimentService> _logger;

        public SentimentService(
            ICoinStorage coinStorage,
            IOrderStorage orderStorage,
            IOrderIndicator buyIndicator,
            IMaintenanceIndicator sentimentIndicator,
            IEventDispatcher eventDispatcher, 
            ILogger<SentimentService> logger)
        {
            _coinStorage = coinStorage;
            _orderStorage = orderStorage;
            _buyIndicator = buyIndicator;
            _sentimentIndicator = sentimentIndicator;
            _eventDispatcher = eventDispatcher;
            _logger = logger;
        }

        public async Task Handle(Candle candle, decimal minimumPercentageChange)
        {
            var coin = await _coinStorage.GetBySymbol(candle.Symbol, candle.Interval);
            if (coin is not null)
            {
                coin.AddCandle(candle, _buyIndicator, _sentimentIndicator);

                await coin.DispatchDomainEvents(_eventDispatcher);
            }
            else
            {
                try
                {
                    var coinToAdd = new Coin(candle.Symbol, candle.Interval, candle.DateTime);
                    coinToAdd.AddCandle(candle, _buyIndicator, _sentimentIndicator);
                    await coinToAdd.DispatchDomainEvents(_eventDispatcher);
                }
                catch
                {
                    _logger.LogWarning($"Failed to create coin. Symbol {candle.Symbol}, Date {candle.DateTime}, Interval {candle.Interval}.");
                }
            }
        }

        public async Task<IEnumerable<Coin>> Get()
        {
            return await _coinStorage.Get();
        }

        public async Task<Coin> Get(string id)
        {
            return await _coinStorage.Get(id);
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderStorage.Get();
        }

        public async Task<IEnumerable<string>> GetSymbols()
        {
            return (await Get()).Select(c => c.Symbol);
        }
    }
}
