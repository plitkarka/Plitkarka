using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plitkarka.Infrastructure.ModelAbstractions;

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
