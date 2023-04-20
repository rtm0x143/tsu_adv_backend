namespace Common.App.Exceptions;

public class CollisionException : ActionFailedException
{
    public CollisionException(string message) : base(message)
    {
    }
}