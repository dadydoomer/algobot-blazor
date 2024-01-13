using Algo.Bot.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Ports
{
    public interface IDateTimeProvider
    {
        TimeSpan TimeToNextFullDateTime(Interval interval, int delayInSeconds = 0, DateTime? now = null);

        DateTime NextFullDateTime(Interval interval, int delayInSeconds = 0, DateTime? now = null);

        DateTime OpenCandleDateTime(Interval interval, DateTime? now = null);

        DateTime CloseCandleDateTime(Interval interval, DateTime? now = null);
    }
}
