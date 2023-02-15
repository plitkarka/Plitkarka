using MediatR;
using Plitkarka.Domain.Handlers.Users;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddMediatR(typeof(AddUserHandler).Assembly);
        
        return services;
    }
}
