namespace Plitkarka.Commons.Exceptions;

public class AuthorizationErrorException : Exception
{
    public AuthorizationErrorException(string message)
        : base(message) { }

    public AuthorizationErrorException(string message, Exception innerException)
        : base(message, innerException) { }

    public AuthorizationErrorException() { }
}
