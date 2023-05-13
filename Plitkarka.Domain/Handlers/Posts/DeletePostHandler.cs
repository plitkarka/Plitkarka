using MediatR;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Posts;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Posts;

public class DeletePostHandler : IRequestHandler<DeletePostRequest>
{
    private User _user { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }

    public DeletePostHandler(
        IRepository<PostEntity> postRepository,
        IContextUserService contextUserService)
    {
        _postRepository = postRepository;
        _user = contextUserService.User;
    }

    public async Task<Unit> Handle(DeletePostRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);

        if (post == null || !post.IsActive)
        {
            throw new ValidationException("No post found");
        }

        if (post.UserId != _user.Id)
        {
            throw new ValidationException("Authorized user has nor rights to delete this post");
        }

        await _postRepository.DeleteAsync(post);

        return Unit.Value;
    }
}
