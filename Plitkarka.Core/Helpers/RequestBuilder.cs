using Polly;

namespace Plitkarka.Core.Helpers;

public class RequestBuilder
{
    private readonly Action _requestToExecute;

    private Action<Exception> _baseExceptionHandler;
    private Policy _builder;

    public RequestBuilder(Action requestToExecute)
    {
        _requestToExecute = requestToExecute ?? throw new ArgumentNullException(nameof(requestToExecute));
        _builder = Policy.NoOp();
    }
    
    public RequestBuilder IgnoreException<TException>()
        where TException : Exception
    {
        _builder = _builder.Wrap(Policy.Handle<TException>().Fallback(() => { }, (ex) => { }));

        return this;
    }
    
    public RequestBuilder HandleException<TException>(Action<Exception> exceptionHandler)
        where TException : Exception
    {
        _builder = _builder.Wrap(Policy.Handle<TException>().Fallback(() => { }, exceptionHandler));

        return this;
    }
    
    
    public RequestBuilder HandleBaseException(Action<Exception> baseExceptionHandler)
    {
        _baseExceptionHandler = baseExceptionHandler;

        return this;
    }

    public void Execute()
    {
        if (_baseExceptionHandler != null)
        {
            _builder = Policy.Handle<Exception>().Fallback(() => { }, _baseExceptionHandler).Wrap(_builder);
        }

        _builder.Execute(_requestToExecute);
    }
}