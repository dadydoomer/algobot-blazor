using Algo.Bot.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Ports.Storage
{
    public interface IRetracementStorage
    {
        Task<List<ExpectedRetracement>> GetAll();
        Task RetracementCompleted(string symbol);
        Task UpdateRetracement(string symbol, decimal athValue, DateTime athDate);
    }
}
