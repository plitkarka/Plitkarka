using Microsoft.OpenApi.Models;
using Plitkarka.Application.Swagger;
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
