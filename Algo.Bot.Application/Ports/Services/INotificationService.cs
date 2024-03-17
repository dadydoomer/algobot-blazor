using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Application.Ports.Services
{
    public interface INotificationService
    {
        Task Notify(string message);
    }
}
