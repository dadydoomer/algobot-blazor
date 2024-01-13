using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.PositiveSentiment.Models.Events
{
    public class CoinApproved : IDomainEvent
    {
        public string Id { get; }

        public DateTime BuyDate { get; }

        public decimal MarkPrice { get; }

        public CoinApproved(string id, DateTime buyDate, decimal markPrice)
        {
            Id = id;
            BuyDate = buyDate;
            MarkPrice = markPrice;
        }
    }
}
