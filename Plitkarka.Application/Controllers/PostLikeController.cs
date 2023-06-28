using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.PaginationModels;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.PostLikes;
using Plitkarka.Domain.Requests.Posts;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/posts/like")]
public class PostLikeController : Controller
{
    private IMediator _mediator { get; init; }

    public PostLikeController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Creates new like",
        Description = $@"
            Creates like at specific post for authorized user.
            Returns 400 if user has already liked this post or post not found
        ")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> CreatePostLike(
        [Required] Guid PostId)
    {
        var id = await _mediator.Send(new CreatePostLikeRequest(PostId));

        return Created(nameof(CreatePostLike), new IdResponse(id));
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Deletes like",
        Description = $@"
            Deletes like at specific post for authorized user.
            Returns 400 if post or like not found
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeletePostLike(
        [Required] Guid PostId)
    {
        await _mediator.Send(new DeletePostLikeRequest(PostId));

        return Accepted();
    }

    [HttpGet("all")]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Returns list of posts liked by specific user",
        Description = $@"
            Returns list of posts liked by specific user, link for the next part of the list and total count of posts.
            If 'Page' is not equal 0 total count of posts will be -1.
            If result list has less number of items then normal the 'NextLink' will be 'String.Empty'.
            'Filter' is Id of the user.
            If filter is not specified returns data related to authorized user.
            Returns 204 if no posts left.
            Returns 400 if user not found
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginationResponse<PostResponse>>> GetLikedPosts(
        [FromQuery] PaginationGuidRequestModel query)
    {
        var response = await _mediator.Send(new GetLikedPostsRequest(
            query.Page,
            query.Filter));

        return Ok(response);
    }
}
