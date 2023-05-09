using MediatR;

namespace Plitkarka.Domain.Requests.PostShares;

public record DeletePostSharingRequest(Guid PostId)
    : IRequest;
