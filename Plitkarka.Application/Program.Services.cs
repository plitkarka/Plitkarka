using MediatR;
using Plitkarka.Domain.Handlers.Users;
using Plitkarka.Infrastructure.Services.ImageService.Service;
using Plitkarka.Infrastructure.Services.ImageService.Models;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Domain.Services.Encryption;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddTransient<IImageService, S3Image>();
        services.AddMediatR(typeof(AddUserHandler).Assembly);
        services.AddTransient<IEncryptionService, Sha256EncryptionService>();
        
        return services;
    }
}
