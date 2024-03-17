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

        public TelegramService(ITelegramBotClient botClient, IOptions<TelegramOptions> options)
        {
            _botClient = botClient;
            _options = options.Value;
        }

        public async Task Notify(string message)
        {
            await _botClient.SendTextMessageAsync(_options.ChatId, message);
        }
    }
}
