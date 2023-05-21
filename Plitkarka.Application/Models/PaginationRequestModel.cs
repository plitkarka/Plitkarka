using System.ComponentModel.DataAnnotations;
using Plitkarka.Commons.Helpers;

namespace Plitkarka.Application.Models;

public record PaginationRequestModel
{
    public int Page { get; set; } = PaginationConsts.DefaultPage;
}
