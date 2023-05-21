﻿using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models;
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
        Description = $@"
            Subscribe user to other one.
            Returns 400 if already subscribed, user not found or user tried to subscribe to himself
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> Subscribe(
        [Required][FromQuery] Guid SubscribeToId)
    {
        var id = await _mediator.Send(new SubscribeRequest(SubscribeToId));

        return Created(nameof(Subscribe), new IdResponse(id));
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Unsubscribe user from other one",
        Description = $@"
            Unsubscribe user from other one.
            Returns 400 if not yet subscibed
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Unsubscribe(
        [Required][FromQuery] Guid UnsubscribeFromId)
    {
        await _mediator.Send(new UnsubscribeRequest(UnsubscribeFromId));

        return Accepted();
    }

    [HttpGet("subscribers")]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Returns list of subscibers",
        Description = $@"
            Returns list of subscibers, link for next part of the list and total count of subscribers.
            If Page is not equal 1 total count of users will be -1.
            If result list has less number of items then normal the next link will be 'String.Empty'.
            Returns 204 if no subscribers left
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<PaginationResponse<UserPreviewResponse>>> GetSubscribers(
        [FromQuery] PaginationGuidRequestModel query)
    {
        var response = await _mediator.Send(new GetSubscribersRequest(
            query.Page,
            query.Filter));

        return response;
    }

    [HttpGet("subscriptions")]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Returns list of subscriptions",
        Description = $@"
            Returns list of subscriptions, link for next part of the list and total count of subscriptions.
            If Page is not equal 1 total count of users will be -1.
            If result list has less number of items then normal the next link will be 'String.Empty'.
            Returns 204 if no subscribers left
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<PaginationResponse<UserPreviewResponse>>> GetSuscriptions(
        [FromQuery] PaginationGuidRequestModel query)
    {
        var response = await _mediator.Send(new GetSubscriptionsRequest(
            query.Page,
            query.Filter));

        return response;
    }
}
