﻿namespace Plitkarka.Domain.Models;

public record RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime Expires { get; set; }

    public bool? IsActive { get; set; }
}
