using Plitkarka.Application.Mapping;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddAutoMapper(typeof(RefreshTokenMappingProfile));

        return services;
    }
}
