﻿using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.Ports;
using Algo.Bot.PositiveSentiment.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Adapters.Indicators
{
    public class BuyWhen2CandlesAreAbove75PercentageBodyRange : IOrderIndicator
    {
        private readonly IMaintenanceIndicator _positiveSentiment = 
            new PositiveSentimentForAllCandlesAbove75PercentageFirstCandle();

        public bool Buy(Coin coin)
        {
            return coin.Candles.Count >= 3
                && coin.Candles.Skip(1).Any(c => c.IsGreenCandle())
                && _positiveSentiment.IsMaintaned(coin);
        }

        public bool Sell(Coin coin)
        {
            throw new NotImplementedException();
        }
    }
}
