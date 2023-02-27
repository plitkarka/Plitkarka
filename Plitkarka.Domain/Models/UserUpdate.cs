namespace Plitkarka.Domain.Models;

public class UserUpdate
{
    public Guid Id { get; set; }

    public string? Login { get; set; } = null;

    public string? Email { get; set; } = null!;

    public string? FirstName { get; set; } = null;

    public string? SecondName { get; set; } = null;

    public DateTime BirthDate { get; set; } = default!;
}
