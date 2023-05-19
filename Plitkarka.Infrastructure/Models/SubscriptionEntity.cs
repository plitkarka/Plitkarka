﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Plitkarka.Infrastructure.ModelAbstractions;

namespace Plitkarka.Infrastructure.Models;

public record SubscriptionEntity : ActivatedEntity
{
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity User { get; set; }

    public Guid SubscribedToId { get; set; }

    public UserEntity SubscribedTo { get; set; }
}