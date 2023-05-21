namespace Plitkarka.Application.Models;

public record PaginationGuidRequestModel : PaginationRequestModel
{
    public Guid Filter { get; set; } = Guid.Empty;
}
