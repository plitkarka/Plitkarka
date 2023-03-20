﻿using MediatR;
using Microsoft.OpenApi.Models;
using Plitkarka.Application.Swagger;
using Plitkarka.Domain.Handlers.Users;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.Authorization;
using Plitkarka.Domain.Services.ContextAccessToken;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.Encryption;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Plitkarka API", Version = "v1" });
            c.OperationFilter<AuthorizationHeaderSwaggerAttribute>();
        });

        services.AddControllers();
        services.AddMediatR(typeof(AddUserHandler).Assembly);

        // HttpContext
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IContextUserService, ContextUserService>();
        services.AddSingleton<IContextAccessTokenService, ContextAccessTokenService>(); 

        // Authentication and Authorization
        services.AddTransient<IAuthenticationService, JwtAuthenticationService>();
        services.AddSingleton<IAuthorizationService, JwtAuthorizationService>();

        // Features
        services.AddSingleton<IEncryptionService, Sha256EncryptionService>();

        return services;
    }
}
