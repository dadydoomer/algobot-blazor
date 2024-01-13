using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Algo.Bot.Infrastructure.Database
{
    public class AlgobotDbContext : DbContext
    {
        public DbSet<CandleEntity> Candles { get; set; }

        public AlgobotDbContext(DbContextOptions<AlgobotDbContext> options)
        : base(options)
        {
        }
    }

    //dotnet ef migrations add InitialCreate --project .\Algo.Bot.Infrastructure\Algo.Bot.Infrastructure.csproj --startup-project .\Algobot\Server\Algobot.Server.csproj
}
