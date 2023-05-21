using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.UserController;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private IMediator _mediator { get; init; }

    public UserController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("image")]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Sets user profile image",
        Description = "Sets user profile image. If user already has image, previous would be overwritten. Throws 400 if image bot provided")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> SetUserImage(
        [FromForm] SetUserImageRequestModel body)
    {
        var id = await _mediator.Send(new SetUserImageRequest(body.Image));

        return Created(nameof(SetUserImage), new IdResponse(id));
    }

    [HttpGet("image")]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Returns URL for profile image of user whose id was provided",
        Description = "Returns URL for profile image of user whose id was provided. If user has no image, returns String.Empty. If user id is not provided, returns image of authorized user. Throws 400 if user not found")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StringResponse>> GetUserProfileImage(
        [FromQuery] Guid userId)
    {
        var url = await _mediator.Send(new GetUserImageRequest(userId));

        return Ok(new StringResponse(url));
    }
}
