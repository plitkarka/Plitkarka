namespace Plitkarka.Domain.Models;

public record User
{
    public Guid Id { get; set; }

    public string Login { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string SecondName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string EmailCode { get; set; } = default!;

    public string Password { get; set; } = default!;

    public int PasswordAttempts { get; set; } = default!;

    public static int PasswordAttemptsCount { get; } = 3;

    public string Salt { get; set; } = default!;

    public DateTime BirthDate { get; set; } = default!;

    public DateTime CreatedDate { get; set; } = default!;

    public DateTime LastLoginDate { get; set; } = default!;

    public bool IsActive { get; set; } = default!;
}
