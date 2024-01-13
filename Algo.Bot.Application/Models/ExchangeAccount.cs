using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Models
{
    public class ExchangeAccount
    {
        public bool CanTrade { get; set; }

        public decimal MarginBalance { get; set; }

        public decimal WalletBalance { get; set; }

        public decimal CrossBalance { get; set; }

        public decimal AvailableBalance { get; set; }

        public decimal UnrealizedProfit { get; set; }
    }
}
