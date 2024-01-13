using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Configuration
{
    public class CoinMarketCapOptions
    {
        public const string CoinMarketCap = "CoinMarketCap";

        public string ApiKey { get; set; }

        public string Url { get; set; } = "https://pro-api.coinmarketcap.com/v1/";

        public string KeyName { get; set; } = "X-CMC_PRO_API_KEY";
    }
}
