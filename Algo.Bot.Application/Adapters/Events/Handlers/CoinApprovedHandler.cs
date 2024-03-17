using Algo.Bot.Application.Ports.Services;
using Algo.Bot.PositiveSentiment.Models.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Algo.Bot.Application.Adapters.Events.Handlers
{
    public class CoinApprovedHandler : INotificationHandler<CoinApproved>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<CoinApprovedHandler> _logger;

        public CoinApprovedHandler(INotificationService notificationService, ILogger<CoinApprovedHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(CoinApproved notification, CancellationToken cancellationToken)
        {
            try
            {
                await _notificationService.Notify(notification.ApprovedMessage);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to create order. Symbol {notification.Symbol}, Interval {notification.Interval}. {e.Message}.");
            }
        }
    }
}
