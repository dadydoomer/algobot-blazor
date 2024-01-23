using Algo.Bot.Application.Adapters.Services;
using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Domain.Ports;
using Algo.Bot.Domain.ValueObject;
using Algobot.Server.BackgroundJobs;
using Hangfire;
using Hangfire.Common;

namespace BlazorAlgoBot.Server.BackgroundJobs
{
    public class CoinDataProviderJob : BackgroundService
    {
        private const string FourHoursExpression = "0 0 0/4 1/1 * ?";
        private const string OneDayExpression = "0 0 0 1/1 * ?";

        private readonly ICoinIntervalJob _coinIntervalJob;

        public CoinDataProviderJob(ICoinIntervalJob coinIntervalJob)
        {
            _coinIntervalJob = coinIntervalJob;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RecurringJob.AddOrUpdate("4h job", () => _coinIntervalJob.DoWork(Interval.FourHour, 3m), FourHoursExpression);
            RecurringJob.AddOrUpdate("1d job", () => _coinIntervalJob.DoWork(Interval.OneDay, 5m), OneDayExpression);
        }
    }
}
