namespace Plitkarka.Application.Models;

public class PaginationFilterRequestModel : PaginationRequestModel
{
    public string Filter { get; set; } = string.Empty;
}
