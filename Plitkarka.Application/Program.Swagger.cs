using Microsoft.OpenApi.Models;
using Plitkarka.Application.Swagger;
using Plitkarka.Domain.Services.ContextAccessToken;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Plitkarka.Application;

public static partial class Program
{
    private static readonly IList<IOperationFilter> Filters = new List<IOperationFilter>();

    public static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        return services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Plitkarka API",
                Version = "v1",
                Description = "Plitkarka ASP .NET Core Web API"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = ContextAccessTokenService.AuthorizationHeaderName,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Access Token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

            options
                .AddOperationFilter<AuthorizationHeaderSwaggerAttribute>()
                .AddSignalR();
        });
    }

    private static SwaggerGenOptions AddOperationFilter<T> (
        this SwaggerGenOptions options) where T : IOperationFilter
    {
        options.OperationFilter<T>();
        Filters.Add(Activator.CreateInstance<T>());

        return options;
    }

    private static SwaggerGenOptions AddSignalR (
        this SwaggerGenOptions options)
    {
        options.AddSignalRSwaggerGen(options =>
        {
            foreach(var filter in Filters)
            {
                options.AddOperationFilter(filter);
            }
        });

        return options;
    }
}
