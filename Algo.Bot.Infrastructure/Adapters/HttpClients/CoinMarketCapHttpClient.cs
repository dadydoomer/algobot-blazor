using Algo.Bot.Domain.ValueObject;
using Algo.Bot.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Adapters.HttpClients
{
    public class CoinMarketCapHttpClient
    {
        private readonly HttpClient _httpClient;

        public CoinMarketCapHttpClient(HttpClient httpClient, IOptions<CoinMarketCapOptions> marketCapOptions)
        {
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add(marketCapOptions.Value.KeyName, marketCapOptions.Value.ApiKey);
        }

        public async Task<List<(string, decimal)>> Get1HSymbols()
        {
            var response = await _httpClient.GetStringAsync($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?limit=100&sort=percent_change_1h&market_cap_min=100000000");

            var result = new List<(string, decimal)>();
            var jsonResult = JObject.Parse(response);
            foreach (var item in jsonResult["data"])
            {
                var symbol = item["symbol"].Value<string>();
                var percantage = item["quote"]["USD"]["percent_change_1h"].Value<decimal>();

                result.Add((symbol + "USDT", percantage));
            }

            return result;
        }

        public async Task<List<(string, decimal)>> Get24HSymbols()
        {
            var response = await _httpClient.GetStringAsync($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?limit=100&sort=percent_change_24h&market_cap_min=100000000");

            var result = new List<(string, decimal)>();
            var jsonResult = JObject.Parse(response);
            foreach (var item in jsonResult["data"])
            {
                var symbol = item["symbol"].Value<string>();
                var percantage = item["quote"]["USD"]["percent_change_24h"].Value<decimal>();

                result.Add((symbol + "USDT", percantage));
            }

            return result;
        }
    }
}
