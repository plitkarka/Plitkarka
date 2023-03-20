﻿namespace Plitkarka.Domain.Models;

public record User
{
    public static int PasswordAttemptsCount { get; } = 3;

    public Guid Id { get; set; }

    public string Login { get; set; }

    public string FirstName { get; set; }

    public string SecondName { get; set; }

    public string Email { get; set; }

    public string EmailCode { get; set; }

    public string Password { get; set; }

    public int PasswordAttempts { get; set; }

    public string Salt { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastLoginDate { get; set; }

    public bool? IsActive { get; set; }


    // ----- Relation properties -----

    public RefreshToken? RefreshToken { get; set; } = null;
}
