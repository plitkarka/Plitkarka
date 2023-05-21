namespace Plitkarka.Domain.ResponseModels;

public record UserPreviewResponse
{
    public Guid Id { get; set; }

    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
}