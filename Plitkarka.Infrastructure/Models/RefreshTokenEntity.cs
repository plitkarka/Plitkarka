using System.ComponentModel.DataAnnotations;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record RefreshTokenEntity : Entity
{
    [Required]
    public string Token { get; set; }

    [Required]
    [MaxLength(32)]
    public string UniqueIdentifier { get; set; }

    [Required]
    public DateTime Expires { get; set; }

    // ----- Relation properties -----

    public Guid? UserId { get; set; }

    public UserEntity? User { get; set; }
}
