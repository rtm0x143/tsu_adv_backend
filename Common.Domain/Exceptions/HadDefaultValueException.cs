namespace Common.Domain.Exceptions;

public class HadDefaultValueException : ArgumentException
{
    public const string DefaultErrorMessage = "Argument can't have default value";
    
    public HadDefaultValueException(string argumentName, Exception? innerException = null) 
        : base(DefaultErrorMessage, argumentName, innerException)
    {
    }
    
    public HadDefaultValueException(string argumentName, string message, Exception? innerException = null) 
        : base(message, argumentName, innerException)
    {
    }
}