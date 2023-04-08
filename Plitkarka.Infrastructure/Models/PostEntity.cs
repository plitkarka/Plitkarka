using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Infrastructure.Models;

public class PostEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(500)]
    public string TextContent { get; set; }

    public DateTime CreationTime { get; set; }

    public bool? IsActive { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }
}
