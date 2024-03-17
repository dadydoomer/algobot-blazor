using Algo.Bot.Application.Adapters.Services;
using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Infrastructure.Adapters;
using Algo.Bot.Infrastructure.Adapters.Externals;
using Algo.Bot.Infrastructure.Adapters.HttpClients;
using Algo.Bot.Infrastructure.Adapters.Storages;
using Algo.Bot.Infrastructure.Configuration;
using Binance.Net;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog;
using Telegram.Bot;

namespace Algo.Bot.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddStorages(services);
            AddHttpClients(services, configuration);
            AddExternalServices(services, configuration);
            AddNotification(services, configuration);
        }

        public static void AddHttpClients(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CoinMarketCapOptions>(configuration.GetSection(CoinMarketCapOptions.CoinMarketCap));

            services.AddHttpClient<CoinMarketCapHttpClient>();
        }

        public static void AddExternalServices(IServiceCollection services, IConfiguration configuration)
        {
            var binanceOptions = new BinanceApiOptions();
            configuration.GetSection(BinanceApiOptions.BinanceApi).Bind(binanceOptions);

            services.AddBinance((restClientOptions, _) => {
                restClientOptions.ApiCredentials = new ApiCredentials(binanceOptions.ApiKey, binanceOptions.SecretKey);
                restClientOptions.LogLevel = LogLevel.Warning;
            });

            services.AddTransient<ICryptocurrencyExchangeService, BinanceExchangeService>();
            services.AddTransient<IPostiveSentimentDataProvider, CoinMarketCapService>();
        }

        public static void AddStorages(IServiceCollection services)
        {
            services.AddSingleton<ICoinStorage, CoinDbStorage>();
            services.AddSingleton<IOrderStorage, OrderStorage>();
        }

        public static void AddNotification(IServiceCollection services, IConfiguration configuration)
        {
            var telegramOptions = new TelegramOptions();
            configuration.GetSection(TelegramOptions.Telegram).Bind(telegramOptions);

            services.AddHttpClient<ITelegramBotClient>(httpClient => new TelegramBotClient(telegramOptions.ApiKey, httpClient));
            services.AddSingleton<INotificationService, TelegramService>();
        }

        public static void AddLogging(IServiceCollection services)
        {
            services.AddLogging(builder => 
            {
                var logger = new LoggerConfiguration()
                            .MinimumLevel.Information()
                            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();

                builder.AddSerilog(logger);
            });
        }
    }
}
