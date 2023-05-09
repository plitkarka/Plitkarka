using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PinedPosts;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.PinedPosts;

public class PinPostHandler : IRequestHandler<PinPostRequest, Guid>
{
    private User _user { get; init; }
    private IRepository<PostPinEntity> _postPinRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }
    private IMapper _mapper { get; init; }

    public PinPostHandler(
        IContextUserService contextUserService,
        IRepository<PostPinEntity> postPinRepository,
        IRepository<PostEntity> postRepository,
        IMapper mapper)
    {
        _user = contextUserService.User;
        _postPinRepository = postPinRepository;
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(PinPostRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);

        if (post == null)
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

        if (existing != null)
        {
            throw new ValidationException("User already pinned this post");
        }

        var newPostPin = new PostPin()
        {
            UserId = _user.Id,
            PostId = request.PostId,
        };
        
        var postPinEntity = _mapper.Map<PostPinEntity>(newPostPin);

        var id = await _postPinRepository.AddAsync(postPinEntity);

        return id;
    }
}
