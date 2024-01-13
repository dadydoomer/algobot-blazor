using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Configuration
{
    public class BinanceApiOptions
    {
        public const string BinanceApi = "BinanceApi";

        public string Url { get; set; }

        public string ApiKey { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;
    }
}
