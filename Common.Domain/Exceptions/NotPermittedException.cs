namespace Common.Domain.Exceptions;

public class NotPermittedException : Exception
{
    public NotPermittedException()
    {
    }

    public NotPermittedException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}