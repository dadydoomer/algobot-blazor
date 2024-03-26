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
        private readonly IRetracementStorage _retracementStorage;
        private readonly IOrderStorage _orderStorage;
        private readonly IOrderIndicator _buyIndicator;
        private readonly IMaintenanceIndicator _sentimentIndicator;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly INotificationService _notificationService;
        private readonly ILogger<SentimentService> _logger;

        public SentimentService(
            ICoinStorage coinStorage,
            IRetracementStorage retracementStorage,
            IOrderStorage orderStorage,
            IOrderIndicator buyIndicator,
            IMaintenanceIndicator sentimentIndicator,
            IEventDispatcher eventDispatcher,
            INotificationService notificationService,
            ILogger<SentimentService> logger)
        {
            _coinStorage = coinStorage;
            _retracementStorage = retracementStorage;
            _orderStorage = orderStorage;
            _buyIndicator = buyIndicator;
            _sentimentIndicator = sentimentIndicator;
            _eventDispatcher = eventDispatcher;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(Candle candle)
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

        public async Task<IEnumerable<ExpectedRetracement>> GetRetracements()
        {
            return await _retracementStorage.GetAll();
        }

        public async Task RetracementComplete(string symbol)
        {
            await _retracementStorage.RetracementCompleted(symbol);
        }

        public async Task RetracementNewAth(string symbol, decimal value, DateTime date)
        {
            await _retracementStorage.UpdateRetracement(symbol, value, date);

            var message = $"Retracement completed. Symbol {symbol}, Value {value}, Date {date}.";
            await _notificationService.Notify(message);
        }
    }
}
