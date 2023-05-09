using MediatR;

namespace Plitkarka.Domain.Requests.PinedPosts;

public record PinPostRequest(Guid PostId) 
    : IRequest<Guid>;
