using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record PostImageEntity : ImageEntity
{
    public Guid? PostId { get; set; }

    public PostEntity? Post { get; set; }
}
