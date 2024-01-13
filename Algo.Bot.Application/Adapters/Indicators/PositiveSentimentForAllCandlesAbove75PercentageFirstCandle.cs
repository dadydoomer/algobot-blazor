using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Indicators
{
    public class PositiveSentimentForAllCandlesAbove75PercentageFirstCandle : IMaintenanceIndicator
    {
        public bool IsMaintaned(Coin coin)
        {
            var firstCandle = coin.Candles[0];
            var seventyFivePercentage = firstCandle.Top75BodyPrice();

            return coin.Candles.All(c =>
            {
                if (c.IsGreenCandle())
                {
                    return c.Open > seventyFivePercentage && c.Close <= firstCandle.High;
                }
                if (c.IsRedCandle())
                {
                    return c.Close > seventyFivePercentage && c.Open <= firstCandle.High;
                }

                return false;
            });
        }

        private bool IsFirstCandle(Candle candle, decimal minimumPercentageChange)
        {
            if (!candle.IsGreenCandle())
            {
                return false;
            }
            if (candle.PercentageBodyChange() < minimumPercentageChange)
            {
                return false;
            }
            if (candle.TopWig() > candle.PercentageBodyChange())
            {
                return false;
            }
            if (!candle.HasLiquity())
            {
                return false;
            }

            return true;
        }
    }
}
