using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record UserImageEntity : ImageEntity
{
    public Guid? UserId { get; set; }

    public UserEntity? User { get; set; }
}
