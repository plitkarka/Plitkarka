using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.PostController;

public record CreatePostRequestModel
{
    [MaxLength(500, ErrorMessage = "Text content is too long. 500 symbols max")]
    public string TextContent { get; set; }
}
