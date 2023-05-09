using MediatR;

namespace Plitkarka.Domain.Requests.PostLikes;

public record DeletePostLikeRequest(Guid PostId) 
    : IRequest;