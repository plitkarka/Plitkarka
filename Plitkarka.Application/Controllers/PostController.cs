using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    [SwaggerOperation(Summary = "Create new Post", Description = "Creates new post for authorized user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdResponse>> CreatePost(
        [FromBody] CreatePostRequestModel body)
    {
        var id = await _mediator.Send(new CreatePostRequest(body.TextContent));

        return Created(nameof(CreatePost), new IdResponse(id));
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Delete Post", 
        Description = "Delete post with specific id for authorized user. Throws 400 in case of: user try to delete someone's post or post not found")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeletePost(
        [Required(ErrorMessage = "Id is required")] Guid PostId)
    {
        await _mediator.Send(new DeletePostRequest(PostId));

        return Accepted();
    }
}
