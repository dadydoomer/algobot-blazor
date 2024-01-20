using Algo.Bot.Application.Adapters.Services;
using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Domain.Ports;
using Algo.Bot.Domain.ValueObject;

namespace BlazorAlgoBot.Server.BackgroundJobs
{
    public class FourHourTimeJob : BackgroundService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<FourHourTimeJob> _logger;
        private int _executionCount;

        public FourHourTimeJob(IPostiveSentimentDataProvider dataProvider,
                                    ICryptocurrencyExchangeService exchange,
                                    ISentimentService sentimentService, 
                                    IDateTimeProvider dateTimeProvider, 
                                    ILogger<FourHourTimeJob> logger)
        {
            DataProvider = dataProvider;
            Exchange = exchange;
            SentimentService = sentimentService;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public IPostiveSentimentDataProvider DataProvider { get; }
        public ICryptocurrencyExchangeService Exchange { get; }
        public ISentimentService SentimentService { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            var minimalPercentageChange = 5m;
            var interval = Interval.FourHour;
            var intervalInSeconds = (int)interval;

            var toFirstRun = _dateTimeProvider.TimeToNextFullDateTime(interval);
            await Task.Delay(toFirstRun);

            await DoWork(interval, minimalPercentageChange);
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(intervalInSeconds));
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork(interval, minimalPercentageChange);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        // Could also be a async method, that can be awaited in ExecuteAsync above
        private async Task DoWork(Interval interval, decimal minimalPercentageChange)
        {
            int count = Interlocked.Increment(ref _executionCount);
            var start = _dateTimeProvider.OpenCandleDateTime(interval);
            var end = _dateTimeProvider.CloseCandleDateTime(interval);

            var latestPositiveSymbols = await DataProvider.GetSymbols(interval, minimalPercentageChange);
            var existingSymbols = await SentimentService.GetSymbols();
            var symbols = MergeSymbols(latestPositiveSymbols, existingSymbols);

            foreach (var symbol in symbols)
            {
                try
                {
                    var candle = await Exchange.GetCandle(symbol, interval, start, end);
                    await SentimentService.Handle(candle, minimalPercentageChange);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }

            _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
        }

        private IEnumerable<string> MergeSymbols(IEnumerable<string> potentialSymbols, IEnumerable<string> existingSymbols)
        {
            var hash = new HashSet<string>(potentialSymbols);
            hash.UnionWith(existingSymbols);

            return hash;
        }
    }
}
