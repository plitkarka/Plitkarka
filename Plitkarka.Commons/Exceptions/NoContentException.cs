namespace Plitkarka.Commons.Exceptions;

public class NoContentException : Exception
{
    public NoContentException(string message)
        : base(message) { }

    public NoContentException(string message, Exception innerException)
        : base(message, innerException) { }

    public NoContentException() { }
}
