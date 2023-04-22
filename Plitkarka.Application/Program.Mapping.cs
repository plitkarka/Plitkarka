using Plitkarka.Application.Mapping;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services
            .AddAutoMapper(typeof(UserMappingProfile))
            .AddAutoMapper(typeof(RefreshTokenMappingProfile))
            .AddAutoMapper(typeof(PostMappingProfile))
            .AddAutoMapper(typeof(PostLikeMappingProfile))
            .AddAutoMapper(typeof(CommentMappingProfile))
            .AddAutoMapper(typeof(CommentLikeMappingProfile));

        return services;
    }
}
