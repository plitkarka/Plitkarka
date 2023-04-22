using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Infrastructure.Models;

public class CommentEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string TextContent { get; set; }

    public DateTime CreationTime { get; set; }

    public bool? IsActive { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }

    public Guid PostId { get; set; }

    public PostEntity? Post { get; set; }

    public ICollection<CommentLikeEntity> CommentLikes { get; set; }
}
