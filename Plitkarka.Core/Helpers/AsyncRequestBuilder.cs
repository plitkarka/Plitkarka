using Polly;

namespace Plitkarka.Core.Helpers;

public class AsyncRequestBuilder
{
    private readonly Func<Task> _requestToExecute;

    private Action<Exception> _baseExceptionHandler;
    private AsyncPolicy _builder;

    public AsyncRequestBuilder(Func<Task> requestToExecute)
    {
        _requestToExecute = requestToExecute ?? throw new ArgumentNullException(nameof(requestToExecute));
        _builder = Policy.NoOpAsync();
    }
    
    public AsyncRequestBuilder IgnoreException<TException>()
        where TException : Exception
    {
        _builder = _builder.WrapAsync(Policy.Handle<TException>().FallbackAsync(
            (token) => Task.CompletedTask,
            (ex) => Task.CompletedTask));

        return this;
    }
    
    public AsyncRequestBuilder HandleException<TException>(Action<Exception> exceptionHandler)
        where TException : Exception
    {
        _builder = _builder.WrapAsync(Policy.Handle<TException>().FallbackAsync(
            (token) => Task.CompletedTask,
            async (ex) => exceptionHandler?.Invoke(ex)));

        return this;
    }
    
    public AsyncRequestBuilder HandleBaseException(Action<Exception> baseExceptionHandler)
    {
        _baseExceptionHandler = baseExceptionHandler;

        return this;
    }

    public async Task ExecuteAsync()
    {
        if (_baseExceptionHandler != null)
        {
            _builder = Policy.Handle<Exception>().FallbackAsync(
                (token) => { return Task.CompletedTask; },
                async (ex) => _baseExceptionHandler?.Invoke(ex)).WrapAsync(_builder);
        }

        await _builder.ExecuteAsync(_requestToExecute);
    }
}