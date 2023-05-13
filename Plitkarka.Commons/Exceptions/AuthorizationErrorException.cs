namespace Plitkarka.Commons.Exceptions;

/// <summary>
/// Throws in Plitkarka.Domain.Middlewares.AuthorizationMiddleware if user tries to authorize without confirmed email adress.
/// </summary>
public class AuthorizationErrorException : Exception
{
    public AuthorizationErrorException(string message)
        : base(message) { }

    public AuthorizationErrorException(string message, Exception innerException)
        : base(message, innerException) { }

    public AuthorizationErrorException() { }
}
