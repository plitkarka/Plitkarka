using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PostLikes;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.PostLikes;

public class CreatePostLikeHandler : IRequestHandler<CreatePostLikeRequest, Guid>
{
    private User _user { get; init; }
    private IRepository<PostLikeEntity> _postLikeRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }
    private IMapper _mapper { get; init; }

    public CreatePostLikeHandler(
        IContextUserService contextUserService,
        IRepository<PostLikeEntity> postLikeRepository,
        IRepository<PostEntity> postRepository,
        IMapper mapper)
    {
        _user = contextUserService.User;
        _postLikeRepository = postLikeRepository;
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePostLikeRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);

        if (post == null)
        {
            throw new ValidationException("Post not found");
        }

        PostLikeEntity? existing;

        try
        {
            existing = await _postLikeRepository.GetAll().FirstOrDefaultAsync(
                pl => pl.PostId == request.PostId && pl.UserId == _user.Id);
        }
        catch(Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing != null)
        {
            throw new ValidationException("User already liked this post");
        }

        var newLike = new PostLike()
        {
            UserId = _user.Id,
            PostId = request.PostId,
        };

        var likeEntity = _mapper.Map<PostLikeEntity>(newLike);

        var id = await _postLikeRepository.AddAsync(likeEntity);

        return id;  
    }
}
