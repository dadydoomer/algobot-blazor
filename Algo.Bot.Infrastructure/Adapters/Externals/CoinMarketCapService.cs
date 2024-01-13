using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Domain.ValueObject;
using Algo.Bot.Infrastructure.Adapters.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Adapters.Externals
{
    public class CoinMarketCapService : IPostiveSentimentDataProvider
    {
        private readonly CoinMarketCapHttpClient _marketCapHttpClient;

        public CoinMarketCapService(CoinMarketCapHttpClient marketCapHttpClient)
        {
            _marketCapHttpClient = marketCapHttpClient;
        }

        public async Task<IList<string>> GetSymbols(Interval interval, decimal minimumPercentageChange)
        {
            var result = new List<(string, decimal)>();

            if (interval == Interval.OneMinute
             || interval == Interval.FiveMinutes
             || interval == Interval.FifteenMinutes
             || interval == Interval.OneHour)
            {
                result = await _marketCapHttpClient.Get1HSymbols();
            }
            if (interval == Interval.FourHour
             || interval == Interval.OneDay)
            {
                result = await _marketCapHttpClient.Get24HSymbols();
            }

            return result.OrderByDescending(x => x.Item2).Where(x => x.Item2 >= minimumPercentageChange).Select(x => x.Item1).ToList();
        }
    }
}
