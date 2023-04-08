using AutoMapper;
using MediatR;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Posts;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Posts;

public class CreatePostHandler : IRequestHandler<CreatePostRequest, Guid>
{
    private User _user { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }
    private IMapper _mapper { get; init; }

    public CreatePostHandler(
        IRepository<PostEntity> postRepository,
        IContextUserService contextUserService,
        IMapper mapper)
    {
        _postRepository = postRepository;
        _user = contextUserService.User;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var newPost = new Post()
        {
            TextContent = request.TextContent,
            UserId = _user.Id
        };

        var postEntity = _mapper.Map<PostEntity>(newPost);

        var id = await _postRepository.AddAsync(postEntity);

        return id;
    }
}
