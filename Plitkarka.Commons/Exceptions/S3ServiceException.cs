namespace Plitkarka.Commons.Exceptions;

public class S3ServiceException : Exception
{
    public S3ServiceException(string message)
       : base(message) { }

    public S3ServiceException(string message, Exception innerException)
        : base(message, innerException) { }

    public S3ServiceException() { }
}
