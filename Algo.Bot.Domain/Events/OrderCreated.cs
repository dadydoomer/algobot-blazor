using Algo.Bot.PositiveSentiment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Events
{
    public class OrderCreated : IDomainEvent
    {
        public Guid Id { get; }

        public string Symbol { get; }

        public OrderCreated(Guid id, string symbol)
        {
            Id = id;
            Symbol = symbol;
        }
    }
}
