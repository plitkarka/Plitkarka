using MediatR;

namespace Plitkarka.Domain.Requests.PostLikes;

public record CreatePostLikeRequest(Guid PostId) 
    : IRequest<Guid>;
