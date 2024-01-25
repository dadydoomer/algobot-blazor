using Algo.Bot.Domain.ValueObject;

namespace Algo.Bot.Domain.Ports
{
    public interface IDateTimeProvider
    {
        TimeSpan TimeToNextFullDateTime(Interval interval, int delayInSeconds = 0, DateTime? now = null);

        DateTime NextFullDateTime(Interval interval, int delayInSeconds = 0, DateTime? now = null);

        DateTime OpenCandleDateTime(Interval interval, DateTime? now = null);

        DateTime CloseCandleDateTime(Interval interval, DateTime? now = null);
        
        DateTime Now(DateTime? now = null);
    }
}
