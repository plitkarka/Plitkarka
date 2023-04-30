using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.PostController;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.Posts;
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
    public async Task<ActionResult<Guid>> CreatePost(
        [FromBody] CreatePostRequestModel body)
    {
        var id = await _mediator.Send(new CreatePostRequest(body.TextContent));

        return Ok(id);
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(Summary = "Delete Post", Description = "Delete post with specific id for authorized user")]
    public async Task<ActionResult<Guid>> DeletePost(
        [Required(ErrorMessage = "Id is required")] Guid id)
    {
        await _mediator.Send(new DeletePostRequest(id));

        return Ok();
    }
}
