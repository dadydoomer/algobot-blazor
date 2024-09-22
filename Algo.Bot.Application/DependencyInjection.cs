using Algo.Bot.Application.Adapters.Events;
using Algo.Bot.Application.Adapters.Indicators;
using Algo.Bot.Application.Adapters.Services;
using Algo.Bot.Application.Adapters.Strategies;
using Algo.Bot.Application.Ports.Services;
using Algo.Bot.Domain.Ports;
using Algo.Bot.PositiveSentiment.Models.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<ISentimentService, SentimentService>();
            AddEventHandlers(services);
            AddStrategies(services);
        }

        public static void AddEventHandlers(IServiceCollection services)
        {
            services.AddMediatR(typeof(EventDispatcher));
            services.AddTransient<IEventDispatcher, EventDispatcher>();
        }

        public static void AddStrategies(IServiceCollection services)
        {
            services.AddTransient<IOrderIndicator, BuyWhen2CandlesAreAbove50PercentageBodyRange>();
            services.AddTransient<IMoneyManagementStrategy, LowRiskTakeProfit2StopLoss1>();
            services.AddTransient<IMaintenanceIndicator, PositiveSentimentForAllCandlesAbove50PercentageFirstCandle>();
        }
    }
}
