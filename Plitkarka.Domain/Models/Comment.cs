﻿namespace Plitkarka.Domain.Models;

public record Comment
{
    public Guid Id { get; set; }

    public string TextContent { get; set; }

    public DateTime CreationTime { get; set; }

    public bool IsActive { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public Guid PostId { get; set; }
}