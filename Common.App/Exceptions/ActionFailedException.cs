namespace Common.App.Exceptions;

public class ActionFailedException : Exception
{
    public ActionFailedException()
    {
    }

    public ActionFailedException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}