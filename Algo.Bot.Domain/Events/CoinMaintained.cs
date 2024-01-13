using Algo.Bot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.PositiveSentiment.Models.Events
{
    public class CoinMaintained : IDomainEvent
    {
        public Coin Coin { get; }

        public CoinMaintained(Coin coin)
        {
            Coin = coin;
        }
    }
}
