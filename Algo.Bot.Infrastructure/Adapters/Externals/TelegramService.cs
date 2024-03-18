using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Algo.Bot.Infrastructure.Adapters.Externals
{
    public class TelegramService : INotificationService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly TelegramOptions _options;

        public TelegramService(IOptions<TelegramOptions> options)
        {
            _options = options.Value;
            _botClient = new TelegramBotClient(_options.ApiKey);
        }

        public async Task Notify(string message)
        {
            await _botClient.SendTextMessageAsync(_options.ChatId, message);
        }
    }
}
