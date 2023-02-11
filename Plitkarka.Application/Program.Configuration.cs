using Plitkarka.Application.Configuration;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services
            .AddOptions<MySqlConfiguration>()
            .BindConfiguration("MySql")
            .Validate(option => 
                !string.IsNullOrEmpty(option.ConnectionString))
            .ValidateOnStart();

        return services;
    }
}
