using Algo.Bot.Domain.Ports;
using Algo.Bot.PositiveSentiment.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator _mediator;

        public EventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish(IDomainEvent @event)
        {
            await _mediator.Publish(@event);
        }
    }
}
