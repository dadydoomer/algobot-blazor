using Algo.Bot.Application.Adapters.Indicators;
using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.Ports;
using Algo.Bot.Domain.ValueObject;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Algobot.Tests.Application
{
    public class PositiveSentimentForAllCandlesAbove50PercentageFirstCandleTests
    {
        IMaintenanceIndicator SystemUnderTests = new PositiveSentimentForAllCandlesAbove50PercentageFirstCandle();

        [Fact]
        public void FirstCandleAways_PositiveSentiment()
        {
            var dateTime = new DateTime(2023, 2, 2, 12, 0, 0);

            var coin = new Coin("BTCUSDT", Interval.FourHour, dateTime.AddHours(4 * 0));
            coin.AddCandle(new Candle(1, 1, 1, 1, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 0)), null, SystemUnderTests);

            SystemUnderTests.IsMaintaned(coin).Should().BeTrue();
        }

        [Fact]
        public void SecondCandle_PositiveSentiment()
        {
            var dateTime = new DateTime(2023, 2, 2, 12, 0, 0);

            var coin = new Coin("BTCUSDT", Interval.FourHour, dateTime.AddHours(4 * 0));
            coin.AddCandle(new Candle(21487, 21487, 22666, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 0)), null, SystemUnderTests);
            coin.AddCandle(new Candle(22424, 22666, 22542, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 1)), null, SystemUnderTests);

            SystemUnderTests.IsMaintaned(coin).Should().BeTrue();
        }

        [Fact]
        public void ThirdCandle_PositiveSentiment()
        {
            var dateTime = new DateTime(2023, 2, 2, 12, 0, 0);

            var coin = new Coin("BTCUSDT", Interval.FourHour, dateTime.AddHours(4 * 0));
            coin.AddCandle(new Candle(21487, 21487, 22666, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 0)), null, SystemUnderTests);
            coin.AddCandle(new Candle(22424, 22666, 22542, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 1)), null, SystemUnderTests);
            coin.AddCandle(new Candle(22509, 22548, 22638, 22671, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 2)), null, SystemUnderTests);

            SystemUnderTests.IsMaintaned(coin).Should().BeTrue();
        }

        [Fact]
        public void SecondCandle_NotPositiveSentiment()
        {
            var dateTime = new DateTime(2023, 2, 2, 12, 0, 0);

            var coin = new Coin("BTCUSDT", Interval.FourHour, dateTime.AddHours(4 * 0));
            coin.AddCandle(new Candle(21487, 21487, 22666, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 0)), null, SystemUnderTests);
            coin.AddCandle(new Candle(22000, 22666, 22050, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 1)), null, SystemUnderTests);

            SystemUnderTests.IsMaintaned(coin).Should().BeFalse();
        }

        [Fact]
        public void ThirdCandle_NotPositiveSentiment()
        {
            var dateTime = new DateTime(2023, 2, 2, 12, 0, 0);

            var coin = new Coin("BTCUSDT", Interval.FourHour, dateTime.AddHours(4 * 0));
            coin.AddCandle(new Candle(21487, 21487, 22666, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 0)), null, SystemUnderTests);
            coin.AddCandle(new Candle(22424, 22666, 22542, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 1)), null, SystemUnderTests);
            coin.AddCandle(new Candle(22000, 22666, 22050, 22756, Interval.FourHour, "BTCUSDT", dateTime.AddHours(4 * 1)), null, SystemUnderTests);

            SystemUnderTests.IsMaintaned(coin).Should().BeFalse();
        }
    }
}
