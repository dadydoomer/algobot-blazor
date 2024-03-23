using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Indicators
{
    public class PositiveSentimentForAllCandlesAbove50PercentageFirstCandle : IMaintenanceIndicator
    {
        public bool IsMaintaned(Coin coin)
        {
            var firstCandle = coin.Candles.First();
            var initialCandlePercentage = firstCandle.HalfBodyPrice();

            if (coin.Candles.Count == 1)
            {
                return true;
            }

            return coin.Candles.Skip(1).All(c =>
            {
                if (c.IsGreenCandle())
                {
                    return c.Open > initialCandlePercentage && c.Close <= firstCandle.High;
                }
                if (c.IsRedCandle())
                {
                    return c.Close > initialCandlePercentage && c.Open <= firstCandle.High;
                }

                return false;
            });
        }
    }
}
