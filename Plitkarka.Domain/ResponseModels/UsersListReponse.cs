namespace Plitkarka.Domain.ResponseModels;

public record UserPreviewResponse
{
    public Guid Id { get; set; }

    public string Login { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
}

public record UsersListResponse
{
    public string NextLink { get; set; }

    public int TotalCount { get; set; }

    public IList<UserPreviewResponse> Users { get; set; }
}
