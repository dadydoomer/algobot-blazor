using Algo.Bot.Application.Models;
using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Domain.Exceptions;
using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.ValueObject;
using Algo.Bot.Infrastructure.Converters;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;

namespace Algo.Bot.Infrastructure.Adapters.HttpClients
{
    public class BinanceExchangeService : ICryptocurrencyExchangeService
    {
        private readonly IBinanceClient _binanceClient;

        public BinanceExchangeService(IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }

        public async Task<ExchangeAccount> GetAccountInfo()
        {
            var info = await _binanceClient.UsdFuturesApi.Account.GetAccountInfoAsync();
            if (info != null 
             && info.Data != null)
            {
                return new ExchangeAccount
                {
                    AvailableBalance = info.Data.AvailableBalance,
                    CanTrade = info.Data.CanTrade,
                    CrossBalance = info.Data.TotalCrossWalletBalance,
                    MarginBalance = info.Data.TotalMarginBalance,
                    UnrealizedProfit = info.Data.TotalUnrealizedProfit,
                    WalletBalance = info.Data.TotalWalletBalance
                };
            }


            throw new NotFoundException("Account info not found.");
        }

        public async Task<IEnumerable<ExchangeOrder>> GetOpenOrders()
        {
            var ordersData = await _binanceClient.UsdFuturesApi.Trading.GetOpenOrdersAsync();
            if (ordersData != null 
             && ordersData.Data != null
             && ordersData.Data.Any())
            {
                return ordersData.Data.Select(d => new ExchangeOrder 
                {
                    Id = d.Id,
                    Symbol = d.Symbol,
                    Price = d.Price,
                    Quantity = d.Quantity
                });
            }

            return new List<ExchangeOrder>();
        }

        public async Task<IEnumerable<ExchangeTrade>> GetOpenTrades(string symbol)
        {
            var ordersData = await _binanceClient.UsdFuturesApi.Trading.GetUserTradesAsync(symbol);

            if (ordersData != null 
             && ordersData.Data.Any())
            {
                return ordersData.Data.Select(d => new ExchangeTrade
                {
                    Id = d.Id,
                    OrderId = d.OrderId,
                    Price = d.Price,
                    Profit = d.RealizedPnl,
                    Quantity = d.Quantity,
                    Symbol = d.Symbol,
                    Timestamp = d.Timestamp
                });
            }

            return new List<ExchangeTrade>();
        }

        public async Task<(int pricePrecision, int quantityPrecision)> GetSymbolPrecisions(string symbol)
        {
            var info = await _binanceClient.UsdFuturesApi.ExchangeData.GetExchangeInfoAsync();

            if (info != null 
             && info.Success)
            {
                var symbolFilter = info.Data.Symbols.First(s => s.Name == symbol);

                return (symbolFilter.PricePrecision, symbolFilter.QuantityPrecision);
            }

            throw new NotFoundException($"Precision for {symbol} not found.");
        }

        public async Task<decimal> GetAccountBalance(string symbol)
        {
            var balance = await _binanceClient.UsdFuturesApi.Account.GetBalancesAsync();

            return balance.Data.First(x => x.Asset == symbol).AvailableBalance;
        }

        public async Task<int> ChangeLeverage(int leverageValue, string symbol)
        {
            var result = await _binanceClient.UsdFuturesApi.Account.ChangeInitialLeverageAsync(symbol, leverageValue);

            return result.Data.Leverage;
        }

        public async Task ChangeModeToIsolated(string symbol)
        {
            await _binanceClient.UsdFuturesApi.Account.ChangeMarginTypeAsync(symbol, FuturesMarginType.Isolated);
        }

        public async Task ChangeModeToCross(string symbol)
        {
            await _binanceClient.UsdFuturesApi.Account.ChangeMarginTypeAsync(symbol, FuturesMarginType.Cross);
        }

        public async Task<long> CreateMarketOrder(
            string symbol,
            decimal quantity)
        {
            var createdOrder = await _binanceClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                symbol,
                OrderSide.Buy,
                FuturesOrderType.Market,
                quantity);

            if (!createdOrder.Success)
            {
                throw new CreateOrderException($"Failed to create order. Error {createdOrder.Error}.");
            }

            return createdOrder.Data.Id;
        }

        public async Task<long> CreateStopLossOrder(
            string symbol,
            decimal quantity,
            decimal stopPrice)
        {
            var createdOrder = await _binanceClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                symbol,
                OrderSide.Sell,
                FuturesOrderType.StopMarket,
                quantity,
                null,
                null,
                TimeInForce.GoodTillCanceled,
                null,
                null,
                stopPrice);

            if (!createdOrder.Success)
            {
                throw new CreateOrderException($"Failed to create order. Error {createdOrder.Error}.");
            }

            return createdOrder.Data.Id;
        }

        public async Task<long> CreateTakeProfitOrder(
            string symbol,
            decimal quantity,
            decimal stopPrice)
        {
            var createdOrder = await _binanceClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                symbol,
                OrderSide.Sell,
                FuturesOrderType.TakeProfitMarket,
                quantity,
                null,
                null,
                TimeInForce.GoodTillCanceled,
                null,
                null,
                stopPrice);

            if (!createdOrder.Success)
            {
                throw new CreateOrderException($"Failed to create order. Error {createdOrder.Error}.");
            }

            return createdOrder.Data.Id;
        }

        public async Task<Candle> GetCandle(string symbol, Interval interval, DateTime start, DateTime end)
        {
            var binanceInterval = ToBinanceInterval.Convert(interval);
            var binanceCandles = await _binanceClient.SpotApi.ExchangeData.GetKlinesAsync(symbol, binanceInterval, start, end, 1);
            if (binanceCandles.Data != null
            && binanceCandles.Data.Any())
            {
                var firstCandle = binanceCandles.Data.First();
                return new Candle
                {
                    Symbol = symbol,
                    Interval = interval,
                    DateTime = firstCandle.OpenTime,
                    Open = firstCandle.OpenPrice,
                    Close = firstCandle.ClosePrice,
                    High = firstCandle.HighPrice,
                    Low = firstCandle.LowPrice,
                };
            }

            throw new NotFoundException($"Candle not found at binance exchange. Symbol {symbol}, Interval {interval}.");
        }

        public async Task<IList<Candle>> GetCandles(string symbol, Interval interval, DateTime start, DateTime end)
        {
            var binanceInterval = ToBinanceInterval.Convert(interval);
            var binanceCandles = await _binanceClient.SpotApi.ExchangeData.GetKlinesAsync(symbol, binanceInterval, start, end);
            if (binanceCandles.Data != null
            && binanceCandles.Data.Any())
            {
                return binanceCandles.Data.Select(firstCandle => new Candle
                {
                    Symbol = symbol,
                    Interval = interval,
                    DateTime = firstCandle.OpenTime,
                    Open = firstCandle.OpenPrice,
                    Close = firstCandle.ClosePrice,
                    High = firstCandle.HighPrice,
                    Low = firstCandle.LowPrice,
                }).ToList();
            }

            throw new NotFoundException($"Candle not found at binance exchange. Symbol {symbol}, Interval {interval}.");
        }

        public async Task<Candle> GetLastCandle(string symbol, Interval interval)
        {
            var binanceInterval = ToBinanceInterval.Convert(interval);
            var binanceCandles = await _binanceClient.SpotApi.ExchangeData.GetKlinesAsync(symbol, binanceInterval);
            if (binanceCandles.Data != null
            && binanceCandles.Data.Any())
            {
                var firstCandle = binanceCandles.Data.First();
                return new Candle
                {
                    Symbol = symbol,
                    Interval = interval,
                    DateTime = firstCandle.OpenTime,
                    Open = firstCandle.OpenPrice,
                    Close = firstCandle.ClosePrice,
                    High = firstCandle.HighPrice,
                    Low = firstCandle.LowPrice,
                };
            }

            throw new NotFoundException($"Candle not found at binance exchange. Symbol {symbol}, Interval {interval}.");
        }
    }
}
