using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.CommentController;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.Comments;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentController : Controller
{
    private IMediator _mediator { get; init; }

    public CommentController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Creates new comment",
        Description = "Creates comment at specific post for authorized user. Throws 400 if post not found")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> CreateCommentLike(
        [FromBody] CreateCommentRequestModel body)
    {
        var id = await _mediator.Send(new CreateCommentRequest(body.PostId,body.TextContent));

        return Created(nameof(CreateCommentLike), new IdResponse(id));
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Deletes comment", 
        Description = "Deletes comment at specific post for authorized user. Throws 400 in if user try to delete someone's comment or comment not found")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteCommentLike(
        [Required] Guid CommentId)
    {
        await _mediator.Send(new DeleteCommentRequest(CommentId));

        return Accepted();
    }
}
