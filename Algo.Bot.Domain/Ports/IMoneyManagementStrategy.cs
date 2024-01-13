using Algo.Bot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Ports
{
    public interface IMoneyManagementStrategy
    {
        decimal StoppLoss(Coin coin);

        decimal TakeProfit(Coin coin);

        int Leverage();

        decimal TotalAmount(Coin coin);

        decimal Position(Coin coin);

        decimal Quantity(Coin coin);
    }
}
