using Common.Domain.Exceptions;

namespace Backend.Features.Order.Domain.Exceptions;

public class DifferentRestaurantException : ActionFailedException
{
    public const string ErrorMessage = "Order can't contain dishes from different restaurants";

    public DifferentRestaurantException() : base(ErrorMessage)
    {
    }

    public DifferentRestaurantException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}