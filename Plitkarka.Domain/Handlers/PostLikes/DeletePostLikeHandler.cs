using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PostLikes;
using Plitkarka.Domain.Requests.Posts;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.PostLikes;

public class DeletePostLikeHandler : IRequestHandler<DeletePostLikeRequest>
{
    private User _user { get; init; }
    private IRepository<PostLikeEntity> _postLikeRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }

    public DeletePostLikeHandler(
        IContextUserService contextUserService,
        IRepository<PostLikeEntity> postLikeRepository,
        IRepository<PostEntity> postRepository)
    {
        _user = contextUserService.User;
        _postLikeRepository = postLikeRepository;
        _postRepository = postRepository;
    }

    public async Task<Unit> Handle(DeletePostLikeRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);

        if (post == null || !post.IsActive)
        {
            throw new ValidationException("Post not found");
        }

        PostLikeEntity? existing;

        try
        {
            existing = await _postLikeRepository.GetAll().FirstOrDefaultAsync(
                pl => pl.PostId == request.PostId && pl.UserId == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing == null)
        {
            throw new ValidationException("Post like not found");
        }

        await _postLikeRepository.DeleteAsync(existing);

        return Unit.Value;
    }
}
