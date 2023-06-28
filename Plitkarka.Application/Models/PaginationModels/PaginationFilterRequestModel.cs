namespace Plitkarka.Application.Models.PaginationModels;

public record PaginationFilterRequestModel : PaginationRequestModel
{
    public string Filter { get; set; } = string.Empty;
}
