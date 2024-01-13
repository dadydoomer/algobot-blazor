using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Exceptions;
using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Adapters
{
    public class CoinStorage : ICoinStorage
    {
        private readonly IList<Coin> coins = new List<Coin>();

        public Task Delete(string id)
        {
            var toDelete = coins.FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                coins.Remove(toDelete);
            }

            return Task.CompletedTask;
        }

        public Task<bool> Exist(string id)
        {
            return Task.FromResult(coins.Any(x => x.Id == id));
        }

        public Task<Coin> Get(string id)
        {
            var coin = coins.FirstOrDefault(x => x.Id == id);
            if (coin != null)
            {
                return Task.FromResult(coin);
            }

            throw new NotFoundException($"Coin not found. Id {id}.");
        }

        public Task<IList<Coin>> Get()
        {
            return Task.FromResult(coins);
        }

        public Task<Coin> GetBySymbol(string symbol)
        {
            return Task.FromResult(coins.FirstOrDefault(c => c.Symbol == symbol));
        }

        public Task Save(Coin coin)
        {
            var alredyExist = coins.Any(x => x.Id == coin.Id);
            if (alredyExist)
            {
                throw new AlreadyExistException($"Coin with id {coin.Id} already exist.");
            }

            coins.Add(coin);

            return Task.CompletedTask;
        }
    }
}
