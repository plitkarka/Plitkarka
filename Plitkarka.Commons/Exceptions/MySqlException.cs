namespace Plitkarka.Commons.Exceptions;

public class MySqlException : Exception
{
    public MySqlException(string message)
        : base(message) { }

    public MySqlException(string message, Exception innerException)
        : base (message, innerException) { }

    public MySqlException() { }
}
