using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddMyHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["MySql:ConnectionString"];

        services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddMySql(connectionString!);

        return services;
    }
}
