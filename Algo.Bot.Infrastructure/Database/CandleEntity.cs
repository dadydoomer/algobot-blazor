using Algo.Bot.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Database
{
    public class CandleEntity
    {
        public Guid Id { get; set; }

        public string CoinId { get; set; } = string.Empty;

        public string Symbol { get; set; } = string.Empty;

        public Interval Interval { get; set; }

        public decimal Open { get; set; }

        public decimal Close { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public DateTime DateTime { get; set; }
    }
}
