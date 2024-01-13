using Algo.Bot.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.PositiveSentiment.Models
{
    public abstract class EntityBase
    {
        public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

        public void AddDomainEvent(IDomainEvent @event)
        {
            DomainEvents.Add(@event);
        }

        public async Task DispatchDomainEvents(IEventDispatcher dispatcher)
        {
            var events = DomainEvents.ToArray();
            DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
            {
                await dispatcher.Publish(entityDomainEvent);
            }
        }
    }
}
