using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Models
{
    public class ExchangeOrder
    {
        public long Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }
    }
}
