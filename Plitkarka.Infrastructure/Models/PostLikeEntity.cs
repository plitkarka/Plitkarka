using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Plitkarka.Infrastructure.ModelAbstractions;

namespace Plitkarka.Infrastructure.Models;

public record PostLikeEntity : Entity
{ 
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }

    public Guid PostId { get; set; }

    public PostEntity? Post { get; set; }
}
