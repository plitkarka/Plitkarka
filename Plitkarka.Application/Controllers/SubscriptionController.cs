using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.Subscriptions;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/subscription")]
public class SubscriptionController : Controller
{
    private IMediator _mediator { get; init; }

    public SubscriptionController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Subscribe user to other one", 
        Description = "Subscribe user to other one. Throws 400 if already subscribed, user not found or user tried to subscribe to himself")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> Subscribe(
        [Required] Guid SubscribeToId)
    {
        var id = await _mediator.Send(new SubscribeRequest(SubscribeToId));

        return Created(nameof(Subscribe), new IdResponse(id));
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Unsubscribe user from other one",
        Description = "Unsubscribe user from other one. Throws 400 if not yet subscibed")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Unsubscribe(
        [Required] Guid UnsubscribeFromId)
    {
        await _mediator.Send(new UnsubscribeRequest(UnsubscribeFromId));

        return Accepted();
    }
}
