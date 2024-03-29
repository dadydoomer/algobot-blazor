﻿using Algo.Bot.Application.Adapters.Services;
using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Domain.Ports;
using Algo.Bot.Domain.ValueObject;
using BlazorAlgoBot.Server.BackgroundJobs;

namespace Algobot.Server.BackgroundJobs
{
    public class CoinIntervalJob : ICoinIntervalJob
    {
        private readonly IPostiveSentimentDataProvider _dataProvider;
        private readonly ICryptocurrencyExchangeService _exchange;
        private readonly ISentimentService _sentimentService;
        private readonly ILogger<CoinIntervalJob> _logger;

        public CoinIntervalJob(IPostiveSentimentDataProvider dataProvider,
                                    ICryptocurrencyExchangeService exchange,
                                    ISentimentService sentimentService,
                                    ILogger<CoinIntervalJob> logger)
        {
            _dataProvider = dataProvider;
            _exchange = exchange;
            _sentimentService = sentimentService;
            _logger = logger;
        }

        public async Task DoWork(Interval interval, decimal minimalPercentageChange)
        {
            var latestPositiveSymbols = await _dataProvider.GetSymbols(interval, minimalPercentageChange);
            var existingSymbols = await _sentimentService.GetSymbols();
            var symbols = MergeSymbols(latestPositiveSymbols, existingSymbols);

            foreach (var symbol in symbols)
            {
                try
                {
                    var candle = await _exchange.GetLastCandle(symbol, interval);
                    await _sentimentService.Handle(candle);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }

        private IEnumerable<string> MergeSymbols(IEnumerable<string> potentialSymbols, IEnumerable<string> existingSymbols)
        {
            var hash = new HashSet<string>(potentialSymbols);
            hash.UnionWith(existingSymbols);

            return hash;
        }
    }
}
