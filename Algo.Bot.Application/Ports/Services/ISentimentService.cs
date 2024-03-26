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
        Task Handle(Candle candle);

        Task<IEnumerable<string>> GetSymbols();

        Task<IEnumerable<Coin>> Get();

        Task<Coin> Get(string id);

        Task<IEnumerable<Order>> GetOrders();

        Task<IEnumerable<ExpectedRetracement>> GetRetracements();

        Task RetracementComplete(string symbol);

        Task RetracementNewAth(string symbol, decimal value, DateTime date);
    }
}
