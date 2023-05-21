namespace Plitkarka.Domain.ResponseModels;

public record PaginationResponse<T>
{
    public string NextLink { get; set; }

    public int TotalCount { get; set; }

    public IList<T> Items { get; set; }
}
