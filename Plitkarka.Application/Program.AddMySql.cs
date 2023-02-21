using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Plitkarka.Application.Configuration;
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
                options.UseMySql(
                    configuration.ConnectionString,
                    new MySqlServerVersion(new Version(8, 0, 28)));
            })
            .AddTransient<IRepository<UserEntity>, UserRepository>(); 
    }
}
