using System.ComponentModel.DataAnnotations;
using Plitkarka.Commons.Helpers;

namespace Plitkarka.Application.Models.PaginationModels;

public record PaginationRequestModel
{
    public int Page { get; set; } = PaginationConsts.DefaultPage;
}
