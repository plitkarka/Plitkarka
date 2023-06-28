using Microsoft.AspNetCore.SignalR;
using Plitkarka.Domain.Services.ContextUser;

namespace Plitkarka.Application.Hubs.Filters;

public class HubAuthorizationFilter : IHubFilter
{
    protected IContextUserService _contextUserService { get; init; }

    public HubAuthorizationFilter(
       IContextUserService contextUserService)
    {
        _contextUserService = contextUserService;
    }

    public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        // is made to check that user is authorized. Check happens inside 'User' getter
        var user = _contextUserService.User;

        await next(context);
    }

    public async ValueTask<object?> InvokeMethodAsync(
       HubInvocationContext invocationContext,
       Func<HubInvocationContext, ValueTask<object?>> next)
    {
        // is made to check that user is authorized. Check happens inside 'User' getter
        var user = _contextUserService.User;

        return await next(invocationContext);
    }

    public async Task OnDisconnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        // is made to check that user is authorized. Check happens inside 'User' getter
        var user = _contextUserService.User;

        await next(context);
    }
}
