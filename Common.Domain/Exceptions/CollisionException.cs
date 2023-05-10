namespace Common.Domain.Exceptions;

public class CollisionException : ActionFailedException
{
    public CollisionException(string message) : base(message)
    {
    }
}