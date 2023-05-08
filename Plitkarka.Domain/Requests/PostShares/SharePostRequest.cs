using MediatR;

namespace Plitkarka.Domain.Requests.PostShares;

public record SharePostRequest(Guid PostId)
    : IRequest<Guid>;
