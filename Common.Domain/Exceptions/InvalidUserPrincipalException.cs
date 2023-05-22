namespace Common.Domain.Exceptions;

public class InvalidUserPrincipalException : ArgumentException
{
    public InvalidUserPrincipalException() : base("Invalid user principal")
    {
    }
}