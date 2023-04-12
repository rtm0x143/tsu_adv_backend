namespace Common.App.Exceptions;

public class NotPermittedException : Exception
{
    public NotPermittedException(string message) : base(message)
    {
    }
}