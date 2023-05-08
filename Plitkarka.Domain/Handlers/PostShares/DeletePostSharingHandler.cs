using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PostShares;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.PostShares;

public class DeletePostSharingHandler : IRequestHandler<DeletePostSharingRequest>
{
    private User _user { get; init; }
    private IRepository<PostShareEntity> _postShareRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }

    public DeletePostSharingHandler(
        IContextUserService contextUserService,
        IRepository<PostShareEntity> postShareRepository,
        IRepository<PostEntity> postRepository)
    {
        _user = contextUserService.User;
        _postShareRepository = postShareRepository;
        _postRepository = postRepository;
    }

    public async Task<Unit> Handle(DeletePostSharingRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);

        if (post == null || !post.IsActive)
        {
            throw new ValidationException("Post not found");
        }

        PostShareEntity? existing;

        try
        {
            existing = await _postShareRepository.GetAll().FirstOrDefaultAsync(
                pl => pl.PostId == request.PostId && pl.UserId == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing == null)
        {
            throw new ValidationException("Post share not found");
        }

        await _postShareRepository.DeleteAsync(existing);

        return Unit.Value;
    }
}
