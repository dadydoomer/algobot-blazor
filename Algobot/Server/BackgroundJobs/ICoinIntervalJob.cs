using Algo.Bot.Domain.ValueObject;

namespace Algobot.Server.BackgroundJobs
{
    public interface ICoinIntervalJob
    {
        Task DoWork(Interval interval, decimal minimalPercentageChange);
    }
}
