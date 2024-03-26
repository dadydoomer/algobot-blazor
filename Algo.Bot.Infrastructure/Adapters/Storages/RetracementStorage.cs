using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Entity;
using Algo.Bot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Infrastructure.Adapters.Storages
{
    public class RetracementStorage : IRetracementStorage
    {
        private readonly IDbContextFactory<AlgobotDbContext> _contextFactory;

        public RetracementStorage(IDbContextFactory<AlgobotDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<ExpectedRetracement>> GetAll()
        {
            var result = new List<ExpectedRetracement>();
            using (var db = _contextFactory.CreateDbContext())
            {
                var retracements = await db.Retracements.Where(x => !x.Completed).AsNoTracking().ToListAsync();
                foreach (var retracement in retracements)
                {
                    result.Add(new ExpectedRetracement 
                    {
                        Symbol = retracement.Symbol,
                        RetracementSize = retracement.Retracement,
                        AthValue = retracement.AthValue,
                        AthDate = retracement.AthDate,
                        Completed = retracement.Completed
                    });
                }
            }

            return result;
        }

        public async Task RetracementCompleted(string symbol)
        {
            using (var db = _contextFactory.CreateDbContext())
            {
                var retracement = await db.Retracements.FirstOrDefaultAsync(x => x.Symbol == symbol);
                if (retracement != null)
                {
                    retracement.Completed = true;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateRetracement(string symbol, decimal athValue, DateTime athDate)
        {
            using (var db = _contextFactory.CreateDbContext())
            {
                var retracement = await db.Retracements.FirstOrDefaultAsync(x => x.Symbol == symbol);
                if (retracement != null)
                {
                    retracement.AthValue = athValue;
                    retracement.AthDate = athDate;
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
