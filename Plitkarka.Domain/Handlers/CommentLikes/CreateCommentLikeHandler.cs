using AutoMapper;
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

public class CreateCommentLikeHandler : IRequestHandler<CreateCommentLikeRequest, Guid>
{
    private User _user { get; init; }
    private IRepository<CommentLikeEntity> _commentLikeRepository { get; init; }
    private IRepository<CommentEntity> _commentRepository { get; init; }
    private IMapper _mapper { get; init; }

    public CreateCommentLikeHandler(
        IContextUserService contextUserService,
        IRepository<CommentLikeEntity> commentLikeRepository,
        IRepository<CommentEntity> commentRepository,
        IMapper mapper)
    {
        _user = contextUserService.User;
        _commentLikeRepository = commentLikeRepository;
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateCommentLikeRequest request, CancellationToken cancellationToken)
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
                pl => pl.CommentId == request.CommentId && pl.UserId == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing != null)
        {
            throw new ValidationException("User already liked this comment");
        }

        var newLike = new CommentLike()
        {
            UserId = _user.Id,
            CommentId = request.CommentId,
        };

        var likeEntity = _mapper.Map<CommentLikeEntity>(newLike);

        var id = await _commentLikeRepository.AddAsync(likeEntity);

        return id;
    }
}
