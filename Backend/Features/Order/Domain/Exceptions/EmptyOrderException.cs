using Common.App.Exceptions;

namespace Backend.Features.Order.Domain.Exceptions;

public class EmptyOrderException : UnsuitableDataException
{
    public const string ErrorMessage = "Order must contain at least 1 dish";
    
    public EmptyOrderException(string message = ErrorMessage) : base(message)
    {
    }
}