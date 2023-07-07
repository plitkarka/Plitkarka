namespace Plitkarka.Domain.ResponseModels;

public record CommentResponse
{
    public Guid CommentId { get; set; }

    public string TextContent { get; set; }

    public int LikesCount { get; set; }

    public bool IsLiked { get; set; }

    public UserPreviewResponse UserPreview { get; set; }
}
