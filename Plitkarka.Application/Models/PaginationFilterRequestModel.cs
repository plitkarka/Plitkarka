namespace Plitkarka.Application.Models;

public record PaginationFilterRequestModel : PaginationRequestModel
{
    public string Filter { get; set; } = string.Empty;
}
