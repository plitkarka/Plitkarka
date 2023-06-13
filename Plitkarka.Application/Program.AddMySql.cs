using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Plitkarka.Commons.Configuration;
using Plitkarka.Infrastructure;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Application;

public static partial class Program
{
    public static IServiceCollection AddMySql(this IServiceCollection services)
    {
        return services
            .AddDbContext<MySqlDbContext>((provider, options) =>
            {
                var configuration = provider.GetRequiredService<IOptions<MySqlConfiguration>>().Value;
                options
                    .UseMySql(
                        configuration.ConnectionString,
                        new MySqlServerVersion(new Version(8, 0, 28)))
                    .ConfigureWarnings(wc => wc.Ignore(RelationalEventId.BoolWithDefaultWarning));
            })
            .AddTransient<IRepository<UserEntity>, UserRepository>()
            .AddTransient<IRepository<RefreshTokenEntity>, RefreshTokenRepository>() 
            .AddTransient<IRepository<PostImageEntity>, PostImageRepository>()
            .AddTransient<IRepository<UserImageEntity>, UserImageRepository>()
            .AddTransient<IRepository<PostEntity>, PostRepository>()
            .AddTransient<IRepository<SubscriptionEntity>, SubscriptionRepository>()
            .AddTransient<IRepository<PostLikeEntity>, PostLikeRepository>()
            .AddTransient<IRepository<CommentEntity>, CommentRepository>()
            .AddTransient<IRepository<CommentLikeEntity>, CommentLikeRepository>()
            .AddTransient<IRepository<PostPinEntity>, PostPinRepository>()
            .AddTransient<IRepository<PostShareEntity>, PostShareRepository>()
            .AddTransient<IRepository<HubConnectionEntity>, HubConnectionRepository>()
            .AddTransient<IRepository<ChatUserConfigurationEntity>, ChatUserConfigurationRepository>()
            .AddTransient<IRepository<ChatEntity>, ChatRepository>()
            .AddTransient<IRepository<ChatMessageEntity>, ChatMessageRepository>()
            .AddTransient<IRepository<ChatMessageImageEntity>, ChatMessageImageRepository>(); 
    }
}
