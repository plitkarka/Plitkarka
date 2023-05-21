using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure;

internal static class ModelBuilderExtensions
{
    private const string DateFunction = "(CURRENT_TIMESTAMP)";
    private const bool DefaultState = true;

    public static ModelBuilder SetupEntity<T>(this ModelBuilder modelBuilder) where T : Entity
    {
        modelBuilder
            .Entity<T>().Property(e => e.CreationTime).HasDefaultValueSql(DateFunction);

        return modelBuilder;
    }

    public static ModelBuilder SetupActivatedEntity<T>(this ModelBuilder modelBuilder) where T : ActivatedEntity
    {
        modelBuilder
            .SetupEntity<T>()
            .Entity<T>().Property(e => e.IsActive).HasDefaultValue(DefaultState);

        return modelBuilder;
    }
}
