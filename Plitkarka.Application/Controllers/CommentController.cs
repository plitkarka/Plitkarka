using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.CommentController;
using Plitkarka.Application.Models.PaginationModels;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.Comments;
using Plitkarka.Domain.Requests.Posts;
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
        Description = $@"
            Creates comment at specific post for authorized user.
            Returns 400 if post not found
        ")]
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
        Description = $@"
            Deletes comment at specific post for authorized user.
            Returns 400 in if user try to delete someone's comment or comment not found
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteCommentLike(
        [Required] Guid CommentId)
    {
        await _mediator.Send(new DeleteCommentRequest(CommentId));

        return Accepted();
    }

    [HttpGet]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Returns list of comments for specific post",
        Description = $@"
            Returns list of comments for specific post, link for the next part of the list and total count of comments.
            If 'Page' is not equal 0 total count of comments will be -1.
            If result list has less number of items then normal the 'NextLink' will be 'String.Empty'.
            'Filter' is Id of the post.
            Returns 204 if no comments left.
            Returns 400 if post not found
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPosts(
        [FromQuery] PaginationGuidRequiredRequestModel query)
    {
        var response = await _mediator.Send(new GetPostCommentsRequest(
            query.Page,
            query.Filter));

        return Ok(response);
    }
}
