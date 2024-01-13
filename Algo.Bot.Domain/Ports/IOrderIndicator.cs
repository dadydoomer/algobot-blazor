using Algo.Bot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.PositiveSentiment.Models.Services
{
    public interface IOrderIndicator
    {
        bool Buy(Coin coin);

        bool Sell(Coin coin);
    }
}
