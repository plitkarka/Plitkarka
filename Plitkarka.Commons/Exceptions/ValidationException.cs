namespace Plitkarka.Commons.Exceptions;

public class ValidationException : Exception
{
    public string? ParamName { get; set; } = null;

    public ValidationException(string message)
        : base(message) { }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException) { }

    public ValidationException(string message, string paramName)
       : base(message) 
    {
        ParamName = paramName;
    }

    public ValidationException() { }
}
