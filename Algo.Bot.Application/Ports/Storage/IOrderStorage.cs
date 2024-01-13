using Algo.Bot.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Ports.Storage
{
    public interface IOrderStorage
    {
        Task<Guid> Save(Order order);

        Task<IList<Order>> Get();

        Task<Order> Get(Guid id);
    }
}
