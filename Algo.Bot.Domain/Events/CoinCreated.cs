using Algo.Bot.PositiveSentiment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Events
{
    public class CoinCreated : IDomainEvent
    {
        public string Id { get; }

        public CoinCreated(string id)
        {
            Id = id;
        }
    }
}
