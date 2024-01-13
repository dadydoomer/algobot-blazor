using Algo.Bot.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Models
{
    public class Candle
    {
        public DateTime DateTime { get; set; }

        public string Symbol { get; set; }

        public Interval Interval { get; set; }

        public decimal Open { get; set; }

        public decimal Close { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public Candle()
        {

        }

        public Candle(decimal low, decimal open, decimal close, decimal high, Interval interval, string symbol, DateTime date)
        {
            Low = low;
            Open = open;
            Close = close;
            High = high;
            Interval = interval;
            Symbol = symbol;
            DateTime = date;

            Validate();
        }

        private void Validate()
        {
            if (High < Low)
            {
                throw new ArgumentException($"High need to be greater than Low. High {High}, Low {Low}.");
            }

            if (IsGreenCandle())
            {
                if (High < Close)
                {
                    throw new ArgumentException($"Green candle High need to be greater than Close. High {High}, Close {Close}.");
                }
                if (Low > Open)
                {
                    throw new ArgumentException($"Green candle Low need to be lower than Open. Open {Open}, Low {Low}.");
                }
            }
            if (IsRedCandle())
            {
                if (High < Open)
                {
                    throw new ArgumentException($"Red candle High need to be lower than Open. High {High}, Open {Open}.");
                }
                if (Low > Close)
                {
                    throw new ArgumentException($"Red candle Low need to be greater than Close. Close {Close}, Low {Low}.");
                }
            }
        }

        public bool IsGreenCandle()
        {
            return Close >= Open;
        }

        public bool IsRedCandle()
        {
            return !IsGreenCandle();
        }

        public decimal PercentageBodyChange()
        {
            if (IsGreenCandle())
            {
                return Math.Round((((Close - Open) / Open) * 100m), 2);
            }
            if (IsRedCandle())
            {
                return (-1) * Math.Round((((Close - Open) / Open) * 100m), 2);
            }

            return 0;
        }

        public decimal PercentageRange()
        {
            if (IsGreenCandle())
            {
                return Math.Round((((High - Low) / Low) * 100m), 2);
            }
            if (IsRedCandle())
            {
                return (-1) * Math.Round((((High - Low) / Low) * 100m), 2);
            }

            return 0;
        }

        public decimal HalfBodyPrice()
        {
            if (IsGreenCandle())
            {
                return (Close - Open) / 2 + Open;
            }
            if (IsRedCandle())
            {
                return (Open - Close) / 2 + Close;
            }

            return 0;
        }

        public decimal Top75BodyPrice()
        {
            if (IsGreenCandle())
            {
                return (Close - HalfBodyPrice()) / 2 + HalfBodyPrice();
            }
            if (IsRedCandle())
            {
                return (Open - HalfBodyPrice()) / 2 + HalfBodyPrice();
            }

            return 0;
        }

        public decimal TopWig()
        {
            if (IsGreenCandle())
            {
                return Math.Round((((High - Close) / Close) * 100m), 2);
            }
            if (IsRedCandle())
            {
                return (-1) * Math.Round((((High - Open) / Open) * 100m), 2);
            }

            return 0;
        }

        public bool HasLiquity()
        {
            var noLiquity = Open == Close
                && Close == High
                && High == Low;

            return !noLiquity;
        }
    }
}
