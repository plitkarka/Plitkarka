using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.CommentController;

public record CreateCommentRequestModel
{
    [Required]
    public Guid PostId { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Text content is too long. 100 symbols max")]
    public string TextContent { get; set; }
}
