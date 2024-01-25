using Algo.Bot.Application.Models;
using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Ports.Services
{
    public interface ICryptocurrencyExchangeService
    {
        Task<ExchangeAccount> GetAccountInfo();

        Task<IEnumerable<ExchangeOrder>> GetOpenOrders();

        Task<IEnumerable<ExchangeTrade>> GetOpenTrades(string symbol);

        Task<decimal> GetAccountBalance(string symbol);

        Task<int> ChangeLeverage(int leverageValue, string symbol);

        Task ChangeModeToIsolated(string symbol);

        Task ChangeModeToCross(string symbol);

        Task<long> CreateMarketOrder(
            string symbol,
            decimal quantity);

        Task<long> CreateStopLossOrder(
            string symbol,
            decimal quantity,
            decimal stopPrice);

        Task<long> CreateTakeProfitOrder(
            string symbol,
            decimal quantity,
            decimal stopPrice);

        Task<Candle> GetCandle(string symbol, Interval interval, DateTime start, DateTime end);
        Task<Candle> GetLastCandle(string symbol, Interval interval);
        Task<IList<Candle>> GetCandles(string symbol, Interval interval, DateTime start, DateTime end);
        Task<(int pricePrecision, int quantityPrecision)> GetSymbolPrecisions(string symbol);
    }
}
