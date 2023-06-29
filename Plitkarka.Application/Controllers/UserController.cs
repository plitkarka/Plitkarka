using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.PaginationModels;
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
        Description = @$"
            Sets user profile image.
            If user already has image, previous will be overwritten.
            Returns 400 if image is not provided
        ")]
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
        Description = @$"
            Returns URL for profile image of user whose id was provided.
            If user has no image, returns 'String.Empty'.
            If user id is not provided, returns image of authorized user.
            Returns 400 if user not found
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StringResponse>> GetUserProfileImage(
        [FromQuery] Guid userId)
    {
        var url = await _mediator.Send(new GetUserImageRequest(userId));

        return Ok(new StringResponse(url));
    }

    [HttpGet]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Returns info of specific user",
        Description = @$"
            Returns info of specific user.
            If user id is not specified returns info of authorized user.
            Returns 400 if user not found
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDataResponse>> GetUserData(
        [FromQuery] Guid userId)
    {
        var response = await _mediator.Send(new GetUserDataRequest(userId));

        return Ok(response);
    }

    [HttpGet("all")]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Returns list of users for preview",
        Description = @$"
            Returns list of users of specific size, link for next part of the list and total count of users.
            If 'Page' is not equal 0 total count of users will be -1.
            If result list has less number of items then normal the 'NextLink' will be 'String.Empty'.
            Returns 204 if no users left
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<PaginationResponse<UserPreviewResponse>>> SearchUsers(
        [FromQuery] PaginationFilterRequestModel query)
{
        var response = await _mediator.Send(new SearchUsersRequest(
            query.Page, 
            query.Filter));

        return Ok(response);
    }

    [HttpPut]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Updates profile of authorized user",
        Description = @$"
            Allows user to change login, name, profile description and outer link
            Throws 400 if no fields was provided or user login is already taken 
            If login is taken has response header 'FailedParam' with the value 'Login'
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDataResponse>> UpdateProfile(
        [FromBody] UpdateProfileRequestModel body)
    {
        var response = await _mediator.Send(new UpdateProfileRequest(
            body.Login,
            body.Name,
            body.Description,
            body.Link));

        return Ok(response);
    }
}
