namespace Plitkarka.Domain.Models;

public class CommentLike
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public bool? IsActive { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public Guid CommentId { get; set; }
}
