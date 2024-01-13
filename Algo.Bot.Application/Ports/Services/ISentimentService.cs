using Algo.Bot.Domain.Entity;
using Algo.Bot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Ports.Services
{
    public interface ISentimentService
    {
        Task Handle(Candle candle, decimal minimumPercentageChange);

        Task<IEnumerable<string>> GetSymbols();

        Task<IEnumerable<Coin>> Get();

        Task<Coin> Get(string id);

        Task<IEnumerable<Order>> GetOrders();
    }
}
