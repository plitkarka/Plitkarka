using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record User : ActivatedLogicModel
{
    public static int PasswordAttemptsCount { get; } = 3;

    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string EmailCode { get; set; }

    public string Password { get; set; }

    public int PasswordAttempts { get; set; }

    public string Salt { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime LastLoginDate { get; set; }

    public string ChangePasswordCode { get; set; }


    // ----- Relation properties -----

    public RefreshToken? RefreshToken { get; set; } = null;
}
