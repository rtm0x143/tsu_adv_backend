namespace Common.App.Exceptions;

public class UnexpectedException : Exception
{
    public UnexpectedException(string message) : base(message)
    {
    }
}