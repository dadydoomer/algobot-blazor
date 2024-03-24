using Algo.Bot.Domain.Models;
using Algo.Bot.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Converters
{
    public static class ToCoin
    {
        public static Coin Map(List<CandleEntity> candles)
        {
            if (candles.Any())
            {
                var firstCandle = candles.First();
                return new Coin(firstCandle.Symbol, firstCandle.Interval, firstCandle.DateTime, candles.Select(c => new Candle(c.Low, c.Open, c.Close, c.High, c.Interval, c.Symbol, c.DateTime)).ToList());
            }

            throw new ArgumentException("Cannot map for empty candles");
        }

        public static List<CandleEntity> Map(Coin coin)
        {
            if (coin.Candles.Any())
            {
                var candles = coin.Candles.Select(candle => new CandleEntity
                {
                    CoinId = coin.Id,
                    Symbol = coin.Symbol,
                    Interval = coin.Interval,
                    Open = candle.Open,
                    High = candle.High,
                    Low = candle.Low,
                    Close = candle.Close,
                    DateTime = candle.DateTime
                });

                return candles.ToList();
            }
            else
            {
                return new List<CandleEntity>();
            }
        }
    }
}
