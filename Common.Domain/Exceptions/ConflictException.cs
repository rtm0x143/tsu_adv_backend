namespace Common.Domain.Exceptions;

public class ConflictException : ActionFailedException
{
    public ConflictException(string message) : base(message)
    {
    }
}