using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.CommentLikes;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/commentLike")]
public class CommentLikeController : Controller
{
    private IMediator _mediator { get; init; }

    public CommentLikeController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Creates new like", 
        Description = "Creates like at specific comment for authorized user. Throws 400 if user has already liked this comment or comment not found")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> CreateCommentLike(
        [Required] Guid CommentId)
    {
        var id = await _mediator.Send(new CreateCommentLikeRequest(CommentId));

        return Created(nameof(CreateCommentLike), new IdResponse(id));
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(Summary = "Deletes like", Description = "Deletes like at specific comment for authorized user. Throw 400 if comment or like not found")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteCommentLike(
        [Required] Guid CommentId)
    {
        await _mediator.Send(new DeleteCommentLikeRequest(CommentId));

        return Accepted();
    }
}
