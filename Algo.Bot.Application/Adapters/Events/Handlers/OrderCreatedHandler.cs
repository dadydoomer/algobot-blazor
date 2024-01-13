using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Events;
using Algo.Bot.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Events.Handlers
{
    public class OrderCreatedHandler : INotificationHandler<OrderCreated>
    {
        private readonly ICryptocurrencyExchangeService _exchangeService;
        private readonly ILogger<OrderCreatedHandler> _logger;
        private readonly IOrderStorage _orderStorage;
        private static readonly ConcurrentDictionary<Guid, bool> finishedOrders = new ConcurrentDictionary<Guid, bool>();

        public OrderCreatedHandler(IOrderStorage orderStorage, ICryptocurrencyExchangeService exchangeService, ILogger<OrderCreatedHandler> logger)
        {
            _exchangeService = exchangeService;
            _orderStorage = orderStorage;
            _logger = logger;
        }

        public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"OrderCreatedHandler. Id {notification.Id} Symbol {notification.Symbol}.");

                var order = await _orderStorage.Get(notification.Id);
                if (order == null)
                {
                    throw new NotFoundException($"Order id {notification.Id}.");
                }

                if (finishedOrders.ContainsKey(order.Id))
                {
                    _logger.LogInformation($"Order with Id {order.Id} already been run.");
                    return;
                }

                var (pricePrecision, quantityPrecision) = await _exchangeService.GetSymbolPrecisions(order.Symbol);
                var quantity = decimal.Round(order.Quantity, quantityPrecision);
                var stopLoss = decimal.Round(order.StopLoss, pricePrecision);
                var takeProfit = decimal.Round(order.TakeProfit, pricePrecision);

                _logger.LogInformation($"QuantityPrecision {quantityPrecision}, Quantity {quantity}, Leverage {order.Leverage}, PricePrecision {pricePrecision}, SL {stopLoss}, TP {takeProfit}.");

                await _exchangeService.ChangeLeverage(order.Leverage, order.Symbol);
                _logger.LogInformation($"ChangeLeverage");
                await _exchangeService.CreateMarketOrder(order.Symbol, quantity);
                _logger.LogInformation($"CreateMarketOrder");
                await _exchangeService.CreateStopLossOrder(order.Symbol, quantity, stopLoss);
                _logger.LogInformation($"CreateStopLossOrder");
                await _exchangeService.CreateTakeProfitOrder(order.Symbol, quantity, takeProfit);
                _logger.LogInformation($"CreateTakeProfitOrder");

                finishedOrders.TryAdd(order.Id, true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to create binance order. Id {notification.Id} {e.Message}.");
            }
        }
    }
}
