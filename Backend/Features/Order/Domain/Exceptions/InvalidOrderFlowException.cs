using Common.Domain.Exceptions;

namespace Backend.Features.Order.Domain.Exceptions;

public class InvalidOrderFlowException : ActionFailedException
{
    public InvalidOrderFlowException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}