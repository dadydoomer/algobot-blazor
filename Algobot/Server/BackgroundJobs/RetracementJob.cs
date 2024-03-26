using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Domain.ValueObject;

namespace Algobot.Server.BackgroundJobs
{
    public class RetracementJob : IRetracementJob
    {
        private readonly ICryptocurrencyExchangeService _exchange;
        private readonly ISentimentService _sentimentService;
        private readonly ILogger<RetracementJob> _logger;

        public RetracementJob(ICryptocurrencyExchangeService exchange, ISentimentService sentimentService, ILogger<RetracementJob> logger)
        {
            _exchange = exchange;
            _sentimentService = sentimentService;
            _logger = logger;

        }

        public async Task DoWork(Interval interval)
        {
            var retracements = await _sentimentService.GetRetracements();
            foreach (var retracement in retracements)
            {
                try
                {
                    var lastCandle = await _exchange.GetLastCandle(retracement.Symbol, interval);
                    if (retracement.Completed)
                    {
                        continue;
                    }

                    if (lastCandle.High > retracement.AthValue)
                    {
                        await _sentimentService.RetracementNewAth(retracement.Symbol, lastCandle.High, lastCandle.DateTime);
                    }
                    else if (lastCandle.Low < retracement.TriggerPrice)
                    {
                        await _sentimentService.RetracementComplete(retracement.Symbol);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }
    }
}
