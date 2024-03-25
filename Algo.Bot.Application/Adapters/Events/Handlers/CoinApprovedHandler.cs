using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Domain.Ports;
using Algo.Bot.PositiveSentiment.Models.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Algo.Bot.Application.Adapters.Events.Handlers
{
    public class CoinApprovedHandler : INotificationHandler<CoinApproved>
    {
        private readonly INotificationService _notificationService;
        private readonly IMoneyManagementStrategy _moneyManagement;
        private readonly ILogger<CoinApprovedHandler> _logger;

        public CoinApprovedHandler(INotificationService notificationService, IMoneyManagementStrategy moneyManagement, ILogger<CoinApprovedHandler> logger)
        {
            _notificationService = notificationService;
            _moneyManagement = moneyManagement;
            _logger = logger;
        }

        public async Task Handle(CoinApproved notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Message {notification.ApprovedMessage}.");
                await _notificationService.Notify(CreateMessage(notification));
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to create order. Symbol {notification.Symbol}, Interval {notification.Interval}. {e.Message}.");
            }
        }

        private string CreateMessage(CoinApproved notification)
        {
            var builder = new StringBuilder();

            builder.AppendLine(notification.ApprovedMessage);
            builder.AppendLine($"Stop loss {_moneyManagement.StoppLoss(notification.Coin)}");
            builder.AppendLine($"Take profit {_moneyManagement.TakeProfit(notification.Coin)}");
            builder.AppendLine($"Quantity {_moneyManagement.Quantity(notification.Coin)}");
            builder.AppendLine($"Position {_moneyManagement.Position(notification.Coin)}");

            return builder.ToString();
        }
    }
}
