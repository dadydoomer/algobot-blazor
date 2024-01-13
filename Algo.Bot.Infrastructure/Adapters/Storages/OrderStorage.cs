using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Entity;
using Algo.Bot.Domain.Exceptions;
using Algo.Bot.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Adapters
{
    public class OrderStorage : IOrderStorage
    {
        private readonly IList<Order> orders = new List<Order>();

        public Task<IList<Order>> Get()
        {
            return Task.FromResult(orders);
        }

        public Task<Order> Get(Guid id)
        {
            if (orders.Any(o => o.Id == id))
            {
                return Task.FromResult(orders.First(o => o.Id == id));
            }

            throw new NotFoundException($"Order with given id not found. Id {id}.");
        }

        public Task<Guid> Save(Order order)
        {
            orders.Add(order);

            return Task.FromResult(order.Id);
        }
    }
}
