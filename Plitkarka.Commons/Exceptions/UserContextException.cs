namespace Plitkarka.Commons.Exceptions;

/// <summary>
/// Throws in UpdateProfileHandler if GetByIdAsunc retruns null, however it shouldn't because it uses aythorized user id
/// </summary>
public class UserContextException : Exception
{
    public UserContextException(string message)
        : base(message) { }

    public UserContextException(string message, Exception innerException)
        : base(message, innerException) { }

    public UserContextException() { }
}
