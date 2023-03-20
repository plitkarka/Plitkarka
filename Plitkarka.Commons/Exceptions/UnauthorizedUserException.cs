namespace Plitkarka.Commons.Exceptions;

public class UnauthorizedUserException : Exception
{
    public UnauthorizedUserException(string message)
        : base(message) { }

    public UnauthorizedUserException(string message, Exception innerException)
        : base(message, innerException) { }

    public UnauthorizedUserException() { }
}
