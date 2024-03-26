using Algo.Bot.Domain.ValueObject;

namespace Algobot.Server.BackgroundJobs
{
    public interface IRetracementJob
    {
        Task DoWork(Interval interval);
    }
}
