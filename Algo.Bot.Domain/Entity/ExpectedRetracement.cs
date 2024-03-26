using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Entity
{
    public class ExpectedRetracement
    {
        public string Symbol { get; set; } = string.Empty;

        public decimal RetracementSize { get; set; }

        public decimal AthValue { get; set; }

        public DateTime AthDate { get; set; }

        public bool Completed { get; set; }

        public decimal TriggerPrice => AthValue - RetracementSize;
    }
}
