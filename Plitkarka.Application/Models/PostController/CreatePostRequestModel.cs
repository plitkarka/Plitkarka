using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Plitkarka.Application.Models.PostController;

public record CreatePostRequestModel
{
    public IFormFile? Image { get; set; }

    [MaxLength(500, ErrorMessage = "Text content is too long. 500 symbols max")]
    public string? TextContent { get; set; }
}
