using MediatR;
using Plitkarka.Domain.Handlers.Users;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Infrastructure.Services.ImageService;
using Plitkarka.Infrastructure.Services.EmailService;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddTransient<IImageService, S3Image>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddMediatR(typeof(AddUserHandler).Assembly);
        services.AddTransient<IEncryptionService, Sha256EncryptionService>();
        
        return services;
    }
}
