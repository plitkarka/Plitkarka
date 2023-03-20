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
            
        services
           .AddOptions<S3Configuration>()
            .BindConfiguration("S3Service")
            .Validate(option =>
                !string.IsNullOrEmpty(option.AccessKey)&&
                !string.IsNullOrEmpty(option.SecretKey)&&
                !string.IsNullOrEmpty(option.BucketName))
            .ValidateOnStart();

        services
          .AddOptions<EmailConfiguration>()
           .BindConfiguration("EmailService")
           .Validate(option =>
               !string.IsNullOrEmpty(option.DisplayName) &&
               !string.IsNullOrEmpty(option.From) &&
               !string.IsNullOrEmpty(option.Host) &&
               !string.IsNullOrEmpty(option.Password) &&
               !string.IsNullOrEmpty(option.UserName))
           .ValidateOnStart();

        return services;
    }
}
