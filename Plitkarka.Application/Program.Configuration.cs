using Plitkarka.Commons.Configuration;

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

        services
            .AddOptions<AuthorizationConfiguration>()
            .BindConfiguration("Authorization")
            .Validate(option =>
                !string.IsNullOrEmpty(option.SecretKey) &&
                option.AccessTokenMinutesLifetime > 0 &&
                option.RefreshTokenDaysLifetime > 0)
            .ValidateOnStart();

        return services;
    }
}
