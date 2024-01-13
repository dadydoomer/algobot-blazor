using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Events;
using Algo.Bot.PositiveSentiment.Models.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Events.Handlers
{
    public class CoinMaintainedHandler : INotificationHandler<CoinMaintained>
    {
        public CoinMaintainedHandler(ICoinStorage coinStorage)
        {
            CoinStorage = coinStorage;
        }

        public ICoinStorage CoinStorage { get; }

        public async Task Handle(CoinMaintained notification, CancellationToken cancellationToken)
        {
            await CoinStorage.Save(notification.Coin);
        }
    }
}
