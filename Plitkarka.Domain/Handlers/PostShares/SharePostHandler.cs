using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PostShares;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.PostShares;

public class SharePostHandler : IRequestHandler<SharePostRequest, Guid>
{
    private User _user { get; init; }
    private IRepository<PostShareEntity> _postShareRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }
    private IMapper _mapper { get; init; }

    public SharePostHandler(
        IContextUserService contextUserService,
        IRepository<PostShareEntity> postShareRepository,
        IRepository<PostEntity> postRepository,
        IMapper mapper)
    {
        _user = contextUserService.User;
        _postShareRepository = postShareRepository;
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(SharePostRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);

        if (post == null)
        {
            throw new ValidationException("Post not found");
        }

        if (post.UserId == _user.Id)
        {
            throw new ValidationException("User can not share his own post");
        }

        PostShareEntity? existing;

        try
        {
            existing = await _postShareRepository.GetAll().FirstOrDefaultAsync(
                ps => ps.PostId == request.PostId && ps.UserId == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing != null)
        {
            if (!existing.IsActive)
            {
                existing.IsActive = true;
                existing.CreationTime = DateTime.UtcNow;

                await _postShareRepository.UpdateAsync(existing);

                return existing.Id;
            }

            throw new ValidationException("User already shared this post");
        }

        var newShare = new PostShare()
        {
            UserId = _user.Id,
            PostId = request.PostId,
        };

        var shareEntity = _mapper.Map<PostShareEntity>(newShare);

        var id = await _postShareRepository.AddAsync(shareEntity);

        return id;
    }
}
