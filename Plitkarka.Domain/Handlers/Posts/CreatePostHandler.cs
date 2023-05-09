using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Posts;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Posts;

public class CreatePostHandler : IRequestHandler<CreatePostRequest, Guid>
{
    private User _user { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }
    private IImageService _imageService { get; init; }
    private IMapper _mapper { get; init; }

    public CreatePostHandler(
        IRepository<PostEntity> postRepository,
        IContextUserService contextUserService,
        IImageService imageService,
        IMapper mapper)
    {
        _postRepository = postRepository;
        _user = contextUserService.User;
        _imageService = imageService;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        if (request.TextContent.IsNullOrEmpty() && request.Image == null)
        {
            throw new ValidationException("Post should contain either text or image");
        }

        Guid imageId = Guid.Empty;

        if (request.Image != null)
        {
            imageId = await _imageService.UploadImageAsync(request.Image);
        }

        var newPost = new Post()
        {
            TextContent = request.TextContent,
            ImageId = imageId,
            UserId = _user.Id
        };

        var postEntity = _mapper.Map<PostEntity>(newPost);

        var id = await _postRepository.AddAsync(postEntity);

        return id;
    }
}
