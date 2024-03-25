using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.PositiveSentiment.Models.Events
{
    public class CoinApproved : IDomainEvent
    {
        public string Symbol { get; private set; } = string.Empty;

        public Interval Interval { get; private set; }

        public string ApprovedMessage { get; } = string.Empty;

        public Coin Coin { get; private set; }

        public CoinApproved(Coin coin)
        {
            Coin = coin;
            Symbol = coin.Symbol;
            Interval = coin.Interval;

            ApprovedMessage = $"Buy signal! Symbol {coin.Symbol}, Inverval {coin.Interval}.";
        }
    }
}
