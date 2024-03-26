using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Algo.Bot.Infrastructure.Database
{
    public class AlgobotDbContext : DbContext
    {
        public DbSet<CandleEntity> Candles { get; set; }

        public DbSet<RetracementEntity> Retracements { get; set; }

        public AlgobotDbContext(DbContextOptions<AlgobotDbContext> options)
        : base(options)
        {
        }

        public AlgobotDbContext()
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CandleEntity>().Property(p => p.Open).HasPrecision(18, 8);
            modelBuilder.Entity<CandleEntity>().Property(p => p.Close).HasPrecision(18, 8);
            modelBuilder.Entity<CandleEntity>().Property(p => p.High).HasPrecision(18, 8);
            modelBuilder.Entity<CandleEntity>().Property(p => p.Low).HasPrecision(18, 8);

            modelBuilder.Entity<RetracementEntity>().Property(p => p.Retracement).HasPrecision(18, 8);
            modelBuilder.Entity<RetracementEntity>().Property(p => p.AthValue).HasPrecision(18, 8);

            base.OnModelCreating(modelBuilder);
        }
    }

    //dotnet ef migrations add InitialCreate --project .\Algo.Bot.Infrastructure\Algo.Bot.Infrastructure.csproj --startup-project .\Algobot\Server\Algobot.Server.csproj
}
