namespace Algo.Bot.Domain.Exceptions
{
    public class CreateOrderException : Exception
    {
        public CreateOrderException(string? message) : base(message)
        {
        }
    }
}
