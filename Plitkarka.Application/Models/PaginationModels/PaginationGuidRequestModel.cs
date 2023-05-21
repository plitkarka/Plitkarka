namespace Plitkarka.Application.Models.PaginationModels;

public record PaginationGuidRequestModel : PaginationRequestModel
{
    public Guid Filter { get; set; } = Guid.Empty;
}
