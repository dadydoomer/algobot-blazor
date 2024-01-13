using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Models
{
    public class ExchangeTrade
    {
        public long Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public long OrderId { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public decimal Profit { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
