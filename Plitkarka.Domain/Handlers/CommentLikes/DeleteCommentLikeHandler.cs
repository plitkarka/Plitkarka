using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.CommentLikes;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.CommentLikes;

public class DeleteCommentLikeHandler : IRequestHandler<DeleteCommentLikeRequest>
{
    private User _user { get; init; }
    private IRepository<CommentLikeEntity> _commentLikeRepository { get; init; }
    private IRepository<CommentEntity> _commentRepository { get; init; }

    public DeleteCommentLikeHandler(
        IContextUserService contextUserService,
        IRepository<CommentLikeEntity> commentLikeRepository,
        IRepository<CommentEntity> commentRepository)
    {
        _user = contextUserService.User;
        _commentLikeRepository = commentLikeRepository;
        _commentRepository = commentRepository;
    }

    public async Task<Unit> Handle(DeleteCommentLikeRequest request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId);

        if (comment == null)
        {
            throw new ValidationException("Comment not found");
        }

        CommentLikeEntity? existing;

        try
        {
            existing = await _commentLikeRepository.GetAll().FirstOrDefaultAsync(
                cl => cl.CommentId == request.CommentId && cl.UserId == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing == null)
        {
            throw new ValidationException("Comment like not found");
}

        await _commentLikeRepository.DeleteAsync(existing);

        return Unit.Value;
    }
}
