using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Exceptions;
using Algo.Bot.Domain.Models;
using Algo.Bot.Domain.ValueObject;
using Algo.Bot.Infrastructure.Converters;
using Algo.Bot.Infrastructure.Database;
using CryptoExchange.Net.CommonObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Adapters.Storages
{
    public class CoinDbStorage : ICoinStorage
    {
        private readonly IDbContextFactory<AlgobotDbContext> _contextFactory;

        public CoinDbStorage(IDbContextFactory<AlgobotDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task Delete(string id)
        {
            using (var db = _contextFactory.CreateDbContext())
            {
                var toDelete = await db.Candles.Where(x => x.CoinId == id).ToListAsync();
                if (toDelete is not null)
                {
                    db.RemoveRange(toDelete);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> Exist(string id)
        {
            using (var db = _contextFactory.CreateDbContext())
            {
                return await db.Candles.AsNoTracking().AnyAsync(x => x.CoinId == id);
            }
        }

        public async Task<Coin?> Get(string id)
        {
            using (var db = _contextFactory.CreateDbContext())
            {
                var candles = await db.Candles.AsNoTracking().Where(x => x.CoinId == id).ToListAsync();
                if (candles.Any())
                {
                    return ToCoin.Map(candles);
                }
                else
                {
                    return null!;
                }
            }
        }

        public async Task<IList<Coin>> Get()
        {
            var result = new List<Coin>();
            using (var db = _contextFactory.CreateDbContext())
            {
                var allCoins = await db.Candles.AsNoTracking().ToListAsync();
                var coins = allCoins.GroupBy(x => x.CoinId);
                foreach (var coin in coins)
                {
                    var coinToAdd = ToCoin.Map(coin.ToList());
                    result.Add(coinToAdd);
                }
            }

            return result;
        }

        public async Task<Coin?> GetBySymbol(string symbol, Interval interval)
        {
            using (var db = _contextFactory.CreateDbContext())
            {
                var candles = await db.Candles.AsNoTracking().Where(x => x.Symbol == symbol && x.Interval == interval).ToListAsync();
                if (candles.Any())
                {
                    return ToCoin.Map(candles);
                }
                else
                {
                    return null!;
                }
            }
        }

        public async Task Save(Coin coin)
        {
            if (!(await Exist(coin.Id)))
            {
                using (var db = _contextFactory.CreateDbContext())
                {
                    var toAdd = ToCoin.Map(coin);

                    await db.Candles.AddRangeAsync(toAdd);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                using (var db = _contextFactory.CreateDbContext())
                {
                    var lastCandle = coin.Candles.Last();

                    await db.Candles.AddAsync(new CandleEntity 
                    { 
                        CoinId = coin.Id, 
                        Symbol = coin.Symbol,
                        Interval = coin.Interval,
                        DateTime = lastCandle.DateTime,
                        High = lastCandle.High,
                        Close = lastCandle.Close,
                        Low = lastCandle.Low,
                        Open = lastCandle.Open,
                    });
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
