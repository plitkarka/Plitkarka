using System.ComponentModel.DataAnnotations;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record RefreshTokenEntity : ActivatedEntity
{
    [Required]
    public string Token { get; set; }

    [Required]
    public DateTime Expires { get; set; }

    // ----- Relation properties -----

    public UserEntity? User { get; set; }
}
