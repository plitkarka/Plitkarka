using System.Net;
using Microsoft.AspNetCore.SignalR;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Application.Hubs.Filters;

public class HubExceptionFilter : IHubFilter
{
    private ILogger<HubExceptionFilter> _logger { get; init; }

    public HubExceptionFilter(
        ILogger<HubExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public async Task OnConnectedAsync(
        HubLifetimeContext context,
        Func<HubLifetimeContext, Task> next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is UnauthorizedUserException)
        {
            throw new HubException("401 Unauthorized");
        }
        catch (Exception ex)
        {
            HandleException(ex);

            throw new HubException(ex.Message);
        }
    }

    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch(Exception ex) when (ex is UnauthorizedUserException)
        {
            return new SignalRResponse
            {
                Code = (int) HttpStatusCode.Unauthorized,
                Message = ex.Message
            };
        }
        catch (Exception ex) when (ex is ValidationException)
        {
            return new SignalRResponse
            {
                Code = (int) HttpStatusCode.BadRequest,
                Message = ex.Message
            };
        }
        catch (Exception ex) when (ex is MySqlException)
        {
            HandleException(ex);

            return new SignalRResponse
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = "Error working with databse"
            };
        }
        catch(Exception ex)
        {
            HandleException(ex);

            return new SignalRResponse
            {
                Code = (int) HttpStatusCode.InternalServerError,
                Message = "Something went wrong"
            };
        }
    }

    public async Task OnDisconnectedAsync(
        HubLifetimeContext context,
        Func<HubLifetimeContext, Task> next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is UnauthorizedUserException)
        {
            throw new HubException("401 Unauthorized");
        }
        catch (Exception ex)
        {
            HandleException(ex);

            throw new HubException(ex.Message);
        }
    }

    private void HandleException(Exception ex)
    {
        _logger.LogError(ex.Message);
    }
}
