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
        public string Symbol { get; set; } = string.Empty;

        public Interval Interval { get; set; }

        public string ApprovedMessage { get; } = string.Empty;

        public CoinApproved(Coin coin)
        {
            Symbol = coin.Symbol;
            Interval = coin.Interval;

            ApprovedMessage = $"Buy signal! Symbol {coin.Symbol}, Inverval {coin.Interval}.";
        }
    }
}
