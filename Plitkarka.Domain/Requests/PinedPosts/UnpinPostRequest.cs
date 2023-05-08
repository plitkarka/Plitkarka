using MediatR;

namespace Plitkarka.Domain.Requests.PinedPosts;

public record UnpinPostRequest(Guid PostId) 
    : IRequest;
