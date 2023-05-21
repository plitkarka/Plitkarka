using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.PaginationModels;
using Plitkarka.Application.Models.PostController;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.Posts;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : Controller
{
    private IMediator _mediator { get; init; }

    public PostController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Create new Post", 
        Description = $@"
            Creates new post for authorized user.
            Returns 400 if post does not contain data
        ")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> CreatePost(
        [FromForm] CreatePostRequestModel body)
    {
        var id = await _mediator.Send(new CreatePostRequest(body.TextContent, body.Image));

        return Created(nameof(CreatePost), new IdResponse(id));
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Delete Post", 
        Description = $@"
            Delete post with specific id for authorized user.
            Returns 400 in case of: user try to delete someone's post or post not found
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeletePost(
        [Required(ErrorMessage = "Id is required")] Guid PostId)
    {
        await _mediator.Send(new DeletePostRequest(PostId));

        return Accepted();
    }

    [HttpGet("all")]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Returns list of posts of specific user",
        Description = $@"
            Returns list of posts of specific user, link for the next part of the list and total count of posts.
            If 'Page' is not equal 0 total count of posts will be -1.
            If result list has less number of items then normal the 'NextLink' will be 'String.Empty'.
            'Filter' is Id of the user.
            If filter is not specified returns data related to authorized user.
            Returns 204 if no posts left.
            Returns 400 if user not found
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginationResponse<PostResponse>>> GetPosts(
        [FromQuery] PaginationGuidRequestModel query)
    {
        var response = await _mediator.Send(new GetPostsRequest(
            query.Page,
            query.Filter));

        return Ok(response);
    }
}
