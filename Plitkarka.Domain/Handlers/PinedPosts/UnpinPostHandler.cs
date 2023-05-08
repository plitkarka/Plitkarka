using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PinedPosts;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.PinedPosts;

public class UnpinPostHandler : IRequestHandler<UnpinPostRequest>
{
    private User _user { get; init; }
    private IRepository<PostPinEntity> _postPinRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }

    public UnpinPostHandler(
        IContextUserService contextUserService,
        IRepository<PostPinEntity> postPinRepository,
        IRepository<PostEntity> postRepository)
    {
        _user = contextUserService.User;
        _postPinRepository = postPinRepository;
        _postRepository = postRepository;
    }

    public async Task<Unit> Handle(UnpinPostRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);

        if (post == null || !post.IsActive)
        {
            throw new ValidationException("Post not found");
        }

        PostPinEntity? existing;

        try
        {
            existing = await _postPinRepository.GetAll().FirstOrDefaultAsync(
                pp => pp.PostId == request.PostId && pp.UserId == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing == null)
        {
            throw new ValidationException("Post pin not found");
        }

        await _postPinRepository.DeleteAsync(existing);

        return Unit.Value;
    }
}
