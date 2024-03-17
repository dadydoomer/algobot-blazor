using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Configuration
{
    public class TelegramOptions
    {
        public const string Telegram = "Telegram";

        public string ChatId { get; set; } = string.Empty;

        public string ApiKey { get; set; } = string.Empty;
    }
}
