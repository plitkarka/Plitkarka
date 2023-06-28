using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.PaginationModels;

public record PaginationGuidRequiredRequestModel : PaginationRequestModel
{
    [Required(ErrorMessage = "Post Id is required")]
    public Guid Filter { get; set; } = Guid.Empty;
}
