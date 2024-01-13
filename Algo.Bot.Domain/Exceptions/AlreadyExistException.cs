using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Bot.Domain.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string? message) : base(message)
        {
        }
    }
}
