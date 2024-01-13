using Algo.Bot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Ports.Storage
{
    public interface ICoinStorage
    {
        Task Save(Coin coin);

        Task Delete(string id);

        Task<Coin?> Get(string id);

        Task<IList<Coin>> Get();

        Task<Coin?> GetBySymbol(string symbol);

        Task<bool> Exist(string id);
    }
}
