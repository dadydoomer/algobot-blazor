using Algo.Bot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Ports
{
    public interface IMaintenanceIndicator
    {
        bool IsMaintaned(Coin coin);
    }
}
