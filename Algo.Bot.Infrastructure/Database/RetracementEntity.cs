using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Database
{
    public class RetracementEntity
    {
        public Guid Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public decimal Retracement { get; set; }

        public decimal AthValue { get; set; }

        public DateTime AthDate { get; set; }

        public bool Completed { get; set; }
    }
}
