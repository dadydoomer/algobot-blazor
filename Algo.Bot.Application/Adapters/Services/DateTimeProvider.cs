using Algo.Bot.Domain.Ports;
using Algo.Bot.Domain.ValueObject;
using MediatR.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now(DateTime? now = null) => now ?? DateTime.UtcNow;

        public DateTime CloseCandleDateTime(Interval interval, DateTime? now = null)
        {
            DateTime Now = now ?? DateTime.UtcNow;

            if (interval == Interval.OneDay)
            {
                return new DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0);
            }
            if (interval == Interval.FourHour)
            {
                var @base = 4;
                return new DateTime(Now.Year, Now.Month, Now.Day + (((Now.Hour / @base) * @base + @base) / 24), ((Now.Hour / @base) * @base + @base) % 24, 0, 0).AddHours(-@base);
            }
            if (interval == Interval.OneHour)
            {
                return new DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, 0, 0);
            }
            if (interval == Interval.FifteenMinutes)
            {
                var @base = 15;
                return new DateTime(Now.Year, Now.Month, Now.Day, Now.Hour + (((Now.Minute / @base) * @base + @base) / 60), ((Now.Minute / @base) * @base + @base) % 60, 0).AddMinutes(-@base);
            }
            if (interval == Interval.FiveMinutes)
            {
                int @base = 5;
                return new DateTime(Now.Year, Now.Month, Now.Day, Now.Hour + (((Now.Minute / @base) * @base + @base) / 60), ((Now.Minute / @base) * @base + @base) % 60, 0).AddMinutes(-@base);
            }
            if (interval == Interval.OneMinute)
            {
                return new DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, 0);
            }

            throw new NotSupportedException($"Given interval is not handled. Interval {interval}.");
        }

        public DateTime OpenCandleDateTime(Interval interval, DateTime? now = null)
        {
            DateTime Now = now ?? DateTime.UtcNow;

            if (interval == Interval.OneDay)
            {
                return new DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0).AddDays(-1);
            }
            if (interval == Interval.FourHour)
            {
                var @base = 4;
                return new DateTime(Now.Year, Now.Month, Now.Day + (((Now.Hour / @base) * @base + @base) / 24), ((Now.Hour / @base) * @base + @base) % 24, 0, 0).AddHours(-2 * @base);
            }
            if (interval == Interval.OneHour)
            {
                return new DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, 0, 0).AddHours(-1);
            }
            if (interval == Interval.FifteenMinutes)
            {
                int @base = 15;
                return new DateTime(Now.Year, Now.Month, Now.Day, Now.Hour + (((Now.Minute / @base) * @base + @base) / 60), ((Now.Minute / @base) * @base + @base) % 60, 0).AddMinutes(-2 * @base);
            }
            if (interval == Interval.FiveMinutes)
            {
                int @base = 5;
                return new DateTime(Now.Year, Now.Month, Now.Day, Now.Hour + (((Now.Minute / @base) * @base + @base) / 60), ((Now.Minute / @base) * @base + @base) % 60, 0).AddMinutes(-2 * @base);
            }
            if (interval == Interval.OneMinute)
            {
                return new DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, 0).AddMinutes(-1);
            }

            throw new NotSupportedException($"Given interval is not handled. Interval {interval}.");
        }

        public TimeSpan TimeToNextFullDateTime(Interval interval, int delayInSeconds = 0, DateTime? now = null)
        {
            DateTime Now = now ?? DateTime.UtcNow;

            return NextFullDateTime(interval, delayInSeconds, Now) - Now;
        }

        private DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        public DateTime NextFullDateTime(Interval interval, int delayInSeconds = 0, DateTime? now = null)
        {
            DateTime Now = now ?? DateTime.UtcNow;
            DateTime nextFullDate = RoundUp(Now, TimeSpan.FromMinutes((int)interval / 60));

            return nextFullDate.AddSeconds(delayInSeconds);
        }
    }
}
