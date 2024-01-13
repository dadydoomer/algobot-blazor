using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.ValueObject
{
    public enum Interval
    {
        OneMinute = 60,

        FiveMinutes = 300,

        FifteenMinutes = 900,

        ThirtyMinutes = 1800,

        OneHour = 3600,

        FourHour = 14400,

        OneDay = 86400,

        OneWeek = 604800,

        OneMonth = 2592000
    }
}
