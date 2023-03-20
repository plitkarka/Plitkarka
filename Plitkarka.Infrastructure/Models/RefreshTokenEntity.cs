using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plitkarka.Infrastructure.Models;

public record RefreshTokenEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public DateTime Expires { get; set; }

    public bool? IsActive { get; set; }


    // ----- Relation properties -----

    public UserEntity? User { get; set; }
}
