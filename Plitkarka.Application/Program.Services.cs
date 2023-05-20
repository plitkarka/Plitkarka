using MediatR;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.Authorization;
using Plitkarka.Domain.Services.ContextAccessToken;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Domain.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Domain.Handlers.Authentication;
using Plitkarka.Domain.Services.Pagination;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddControllers();
        services.AddSwaggerGen();
        services.AddTransient<IImageService, S3Image>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddMediatR(typeof(SignUpHandler).Assembly);

        // HttpContext
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IContextUserService, ContextUserService>();
        services.AddSingleton<IContextAccessTokenService, ContextAccessTokenService>(); 

        // Authentication and Authorization
        services.AddTransient<IAuthenticationService, JwtAuthenticationService>();
        services.AddSingleton<IAuthorizationService, JwtAuthorizationService>();

        // Features
        services.AddSingleton<IEncryptionService, Sha256EncryptionService>();
        services.AddTransient(typeof(IPaginationService<>), typeof(PaginationService<>));

        return services;
    }
}
