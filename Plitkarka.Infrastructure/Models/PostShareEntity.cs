using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record PostShareEntity : ActivatedEntity
{
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }

    public Guid PostId { get; set; }

    public PostEntity? Post { get; set; }
}
