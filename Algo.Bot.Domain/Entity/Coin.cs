using Algo.Bot.Domain.Events;
using Algo.Bot.Domain.Exceptions;
using Algo.Bot.Domain.Ports;
using Algo.Bot.Domain.ValueObject;
using Algo.Bot.PositiveSentiment.Models;
using Algo.Bot.PositiveSentiment.Models.Events;
using Algo.Bot.PositiveSentiment.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Models
{
    public class Coin : EntityBase
    {
        public string Id { get; }

        public string Symbol { get; }

        public Interval Interval { get; }

        public List<Candle> Candles { get; } = new List<Candle>();

        public Coin(string symbol, Interval interval, DateTime startDate)
        {
            Symbol = symbol;
            Interval = interval;
            Id = BuildCoinId(symbol, interval, startDate);

            AddDomainEvent(new CoinCreated(Id));
        }

        public Coin(string symbol, Interval interval, DateTime startDate, List<Candle> candles)
        {
            Symbol = symbol;
            Interval = interval;
            Candles = candles;
            Id = BuildCoinId(symbol, interval, startDate);

            AddDomainEvent(new CoinCreated(Id));
        }

        public void AddCandle(Candle candle, IOrderIndicator orderIndicator, IMaintenanceIndicator sentimentIndicator)
        {
            Candles.Add(candle);

            if (ContinuedInPositiveSentiment(sentimentIndicator))
            {
                AddDomainEvent(new CoinMaintained(this));
            }
            else
            {
                AddDomainEvent(new CoinRejected(Id));
            }
        }

        private bool ContinuedInPositiveSentiment(IMaintenanceIndicator sentimentIndicator)
        {
            return sentimentIndicator.IsMaintaned(this);
        }

        private bool Buy(IOrderIndicator buyIndicator)
        {
            return buyIndicator.Buy(this);
        }

        private bool Sell(IOrderIndicator buyIndicator)
        {
            return buyIndicator.Sell(this);
        }

        private string BuildCoinId(string symbol, Interval interval, DateTime firstCandleDate)
        {
            return $"{symbol}-{interval}";
        }

        public static string BuildCoinIdGenerator(string symbol, Interval interval, DateTime firstCandleDate)
        {
            return $"{symbol}-{interval}";
        }
    }
}
