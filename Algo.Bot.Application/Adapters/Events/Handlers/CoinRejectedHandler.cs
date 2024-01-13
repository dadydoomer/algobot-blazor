using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.PositiveSentiment.Models.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Events.Handlers
{
    public class CoinRejectedHandler : INotificationHandler<CoinRejected>
    {
        public CoinRejectedHandler(ICoinStorage coinStorage)
        {
            CoinStorage = coinStorage;
        }

        public ICoinStorage CoinStorage { get; }

        public async Task Handle(CoinRejected notification, CancellationToken cancellationToken)
        {
            if (await CoinStorage.Exist(notification.Id))
            {
                await CoinStorage.Delete(notification.Id);
            }
        }
    }
}
