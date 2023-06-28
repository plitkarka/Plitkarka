using Microsoft.AspNetCore.SignalR;
using Plitkarka.Application.Hubs;
using Plitkarka.Application.Hubs.Filters;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddSignaR(
        this IServiceCollection services)
    {
        services
            .AddSignalR(options =>
            {
                options.AddFilter<HubExceptionFilter>();
            })
            .AddHubOptions<ChatHub>(options =>
            {
                options.AddFilter<HubAuthorizationFilter>();
            });

        return services;
    }
}
