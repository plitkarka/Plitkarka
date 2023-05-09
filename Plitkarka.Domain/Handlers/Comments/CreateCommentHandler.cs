using AutoMapper;
using MediatR;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Comments;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Comments;

public class CreateCommentHandler : IRequestHandler<CreateCommentRequest, Guid>
{
    private User _user { get; init; }
    private IRepository<CommentEntity> _commentRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }
    private IMapper _mapper { get; init; }

    public CreateCommentHandler(
        IContextUserService contextUserService,
        IRepository<CommentEntity> commentRepository,
        IRepository<PostEntity> postRepository,
        IMapper mapper)
    {
        _user = contextUserService.User;
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId);

        if (post == null)
        {
            throw new ValidationException("Post not found");
        }

        var newComment = new Comment()
        {
            UserId = _user.Id,
            PostId = request.PostId,
            TextContent = request.TextContent
        };

        var commentEntity = _mapper.Map<CommentEntity>(newComment);

        var id = await _commentRepository.AddAsync(commentEntity);

        return id;
    }
}
