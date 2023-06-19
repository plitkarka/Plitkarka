using MediatR;
using Microsoft.AspNetCore.Http;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.ChatHub;

public record SendImageMessageRequest(
    Guid ReceiverId,
    IFormFile Image,
    string Message)
    : IRequest<HubNotificationHandlerResponse>;
