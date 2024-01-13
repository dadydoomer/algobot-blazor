using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.PositiveSentiment.Models.Events
{
    public class CoinRejected : IDomainEvent
    {
        public string Id { get; }

        public CoinRejected(string id)
        {
            Id = id;
        }
    }
}
