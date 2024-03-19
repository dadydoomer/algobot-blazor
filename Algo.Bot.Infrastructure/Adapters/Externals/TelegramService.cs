using Algo.Bot.Application.Adapters.Events.Handlers;
using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Algo.Bot.Infrastructure.Adapters.Externals
{
    public class TelegramService : INotificationService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly TelegramOptions _options;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(IOptions<TelegramOptions> options, ILogger<TelegramService> logger)
        {
            _options = options.Value;
            _logger = logger;
            _botClient = new TelegramBotClient(_options.ApiKey);
        }

        public async Task Notify(string message)
        {
            _logger.LogInformation($"ApiKey {_options.ApiKey}. ChatId {_options.ChatId}.");

            await _botClient.SendTextMessageAsync(_options.ChatId, message);
        }
    }
}
