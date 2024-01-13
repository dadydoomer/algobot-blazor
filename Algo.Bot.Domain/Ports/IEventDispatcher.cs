using Algo.Bot.PositiveSentiment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Ports
{
    public interface IEventDispatcher
    {
        Task Publish(IDomainEvent @event);
    }
}
