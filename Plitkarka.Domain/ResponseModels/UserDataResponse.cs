namespace Plitkarka.Domain.ResponseModels;

public record UserDataResponse
{
    public Guid Id { get; set; }

    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string? Description { get; set; }

    public string? Link { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime LastLoginDate { get; set; }

    public int SubscribersCount { get; set; }

    public int SubscriptionsCount { get; set; }

    public string? ImageUrl { get; set; }
}
