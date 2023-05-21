using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.CommentLikes;
using Plitkarka.Domain.Requests.PostShares;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/posts/share")]
public class PostShareController : Controller
{
    private IMediator _mediator { get; init; }

    public PostShareController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Share post",
        Description = $@"
            Share post for authorized user.
            Returns 400 if user has already shared this post, post not found or user shares his own post
        ")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> SharePost(
        [Required] Guid PostId)
    {
        var id = await _mediator.Send(new SharePostRequest(PostId));

        return Created(nameof(SharePost), new IdResponse(id));
    }

    [HttpDelete]
    [Authorize]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Deletes post sharing",
        Description = $@"
            Deletes post sharing for authorized user.
            Returns 400 if post was not shared or post not found
        ")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteCommentLike(
        [Required] Guid PostId)
    {
        await _mediator.Send(new DeletePostSharingRequest(PostId));

        return Accepted();
    }
}
