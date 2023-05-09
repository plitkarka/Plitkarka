using Microsoft.OpenApi.Models;
using Plitkarka.Application.Swagger;

namespace Plitkarka.Application;

public static partial class Program
{
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

            options.OperationFilter<AuthorizationHeaderSwaggerAttribute>();
        });
    }
}
