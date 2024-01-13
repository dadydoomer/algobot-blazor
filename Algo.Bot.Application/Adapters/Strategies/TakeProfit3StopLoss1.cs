using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Strategies
{
    public class TakeProfit3StopLoss1 : IMoneyManagementStrategy
    {
        readonly decimal StopLossUsdt = 10m;

        public decimal StoppLoss(Coin coin)
        {
            var lastCandle = coin.Candles.Last();
            return lastCandle.Close - StopLossRange(coin);
        }

        public decimal TakeProfit(Coin coin)
        {
            var lastCandle = coin.Candles.Last();
            return lastCandle.Close + 3 * StopLossRange(coin);
        }

        private decimal StopLossRange(Coin coin)
        {
            var lastCandle = coin.Candles.Last();
            var firstCandle = coin.Candles[0];

            return lastCandle.Close - firstCandle.HalfBodyPrice();
        }

        public int Leverage()
        {
            return 5;
        }

        public decimal TotalAmount(Coin coin)
        {
            var lastCandle = coin.Candles.Last();

            var stopLossCandle = new Candle
            {
                Low = StoppLoss(coin),
                Open = lastCandle.Close,
                Close = StoppLoss(coin),
                High = lastCandle.Close
            };

            var stopLossPercentage = stopLossCandle.PercentageBodyChange();

            return 100 * StopLossUsdt / stopLossPercentage;
        }

        public decimal Position(Coin coin)
        {
            return TotalAmount(coin) / Leverage();
        }

        public decimal Quantity(Coin coin)
        {
            var lastPrice = coin.Candles.Last().Close;

            return TotalAmount(coin) / lastPrice;
        }
    }
}
