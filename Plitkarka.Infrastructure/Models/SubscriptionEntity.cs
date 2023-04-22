using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Infrastructure.Models;

public class SubscriptionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public bool? IsActive { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity User { get; set; }

    public Guid SubscribedToId { get; set; }

    public UserEntity SubscribedTo { get; set; }
}
