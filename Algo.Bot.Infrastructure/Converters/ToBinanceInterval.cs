using Algo.Bot.Domain.ValueObject;
using Binance.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Converters
{
    public static class ToBinanceInterval
    {
        public static KlineInterval Convert(Interval interval)
        {
            return interval switch
            {
                Interval.OneMinute => KlineInterval.OneMinute,
                Interval.FiveMinutes => KlineInterval.FiveMinutes,
                Interval.FifteenMinutes => KlineInterval.FifteenMinutes,
                Interval.ThirtyMinutes => KlineInterval.ThreeMinutes,
                Interval.OneHour => KlineInterval.OneHour,
                Interval.FourHour => KlineInterval.FourHour,
                Interval.OneDay => KlineInterval.OneDay,
                Interval.OneWeek => KlineInterval.OneWeek,
                Interval.OneMonth => KlineInterval.OneMonth,
                _ => throw new ArgumentException($"Given interval is not handled. Interval {interval}."),
            };
        }
    }
}
