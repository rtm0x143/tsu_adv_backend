namespace Common.Domain.Exceptions;

public class NotPermittedException : Exception
{
    public NotPermittedException() : base("Not permitted")
    {
    }

    public NotPermittedException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}