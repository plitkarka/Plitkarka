namespace Plitkarka.Domain.ResponseModels;

public record UserPreviewResponse
{
    public Guid UserId { get; set; }

    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string ImageKey { get; set; }

    public string ImageUrl { get; set; }
}