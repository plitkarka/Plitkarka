using MediatR;
using Plitkarka.Domain.Handlers.Users;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.Encryption;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddMediatR(typeof(AddUserHandler).Assembly);
        services.AddTransient<IEncryptionService, Sha256EncryptionService>();
        services.AddTransient<IAuthenticationService, JwtAuthenticationService>();
        
        return services;
    }
}
