using Algo.Bot.Domain.Events;
using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.Ports;
using Algo.Bot.PositiveSentiment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Entity
{
    public class Order : EntityBase
    {
        public Guid Id { set; get; }

        public string CoinId { get; set; }

        public string Symbol { get; set; }

        public DateTime Triggered { get; set; }

        public decimal StopLoss { get; set; }

        public decimal TakeProfit { get; set; }

        public decimal Position { get; set; }

        public int Leverage { get; set; }

        public decimal Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal MarkPrice { get; set; }
    }
}
