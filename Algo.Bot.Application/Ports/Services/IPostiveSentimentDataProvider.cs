using Algo.Bot.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Ports.Services
{
    public interface IPostiveSentimentDataProvider
    {
        Task<IList<string>> GetSymbols(Interval interval, decimal minimumPercentageChange);
    }
}
