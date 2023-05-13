using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Comments;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Comments;

public class DeleteCommentHandler : IRequestHandler<DeleteCommentRequest>
{
    private User _user { get; init; }
    private IRepository<CommentEntity> _commentRepository { get; init; }

    public DeleteCommentHandler(
        IContextUserService contextUserService,
        IRepository<CommentEntity> commentRepository)
    {
        _user = contextUserService.User;
        _commentRepository = commentRepository;
    }

    public async Task<Unit> Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId);

        if (comment == null || !comment.IsActive)
        {
            throw new ValidationException("No comment found");
        }

        if (comment.UserId != _user.Id)
        {
            throw new ValidationException("Authorized user has nor rights to delete this comment");
        }

        await _commentRepository.DeleteAsync(comment);

        return Unit.Value;
    }
}
