using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Entity;
using Algo.Bot.Domain.Events;
using Algo.Bot.Domain.Ports;
using Algo.Bot.PositiveSentiment.Models.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Events.Handlers
{
    public class CoinApprovedHandler : INotificationHandler<CoinApproved>
    {
        public CoinApprovedHandler(
            IOrderStorage orderStorage, 
            ICoinStorage coinStorage, 
            IMoneyManagementStrategy moneyManagement,
            IDateTimeProvider dateTimeProvider,
            IEventDispatcher eventDispatcher,
            ILogger<CoinApprovedHandler> logger)
        {
            OrderStorage = orderStorage;
            CoinStorage = coinStorage;
            MoneyManagement = moneyManagement;
            DateTimeProvider = dateTimeProvider;
            EventDispatcher = eventDispatcher;
            Logger = logger;
        }

        public IOrderStorage OrderStorage { get; }
        public ICoinStorage CoinStorage { get; }
        public IMoneyManagementStrategy MoneyManagement { get; }
        public IDateTimeProvider DateTimeProvider { get; }
        public IEventDispatcher EventDispatcher { get; }
        public ILogger<CoinApprovedHandler> Logger { get; }

        public async Task Handle(CoinApproved notification, CancellationToken cancellationToken)
        {
            var coin = await CoinStorage.Get(notification.Id);
            try
            {
                Logger.LogInformation($"First candle {coin.Candles[0].Low} {coin.Candles[0].Open} {coin.Candles[0].Close} {coin.Candles[0].High}");
                foreach (var candle in coin.Candles)
                {
                    Logger.LogInformation($"Next candle {candle.Low} {candle.Open} {candle.Close} {candle.High}");
                }

                var orderToAdd = new Order
                {
                    Id = Guid.NewGuid(),
                    CoinId = coin.Id,
                    Symbol = coin.Symbol,
                    Triggered = notification.BuyDate,
                    MarkPrice = notification.MarkPrice,
                    Leverage = MoneyManagement.Leverage(),
                    Position = MoneyManagement.Position(coin),
                    TotalAmount = MoneyManagement.TotalAmount(coin),
                    StopLoss = MoneyManagement.StoppLoss(coin),
                    TakeProfit = MoneyManagement.TakeProfit(coin),
                    Quantity = MoneyManagement.Quantity(coin)
                };
                orderToAdd.AddDomainEvent(new OrderCreated(orderToAdd.Id, orderToAdd.Symbol));
                Logger.LogInformation($"SentimentApproved OrderCreated event added.");

                await OrderStorage.Save(orderToAdd);
                Logger.LogInformation($"SentimentApproved order saved.");
                await orderToAdd.DispatchDomainEvents(EventDispatcher);
                Logger.LogInformation($"SentimentApproved order events dispatched.");

                await CoinStorage.Delete(coin.Id);
                Logger.LogInformation($"SentimentApproved coin deleted.");
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to create order. Symbol {coin.Symbol} {e.Message}.");
            }
            finally
            {
                await CoinStorage.Delete(coin.Id);
            }
        }
    }
}
