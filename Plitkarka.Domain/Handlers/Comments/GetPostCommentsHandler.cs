using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Comments;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Comments;

public class GetPostCommentsHandler : IRequestHandler<GetPostCommentsRequest, PaginationResponse<CommentResponse>>
{
    private IRepository<PostEntity> _postRepository { get; init; }
    private IPaginationService<CommentEntity> _paginationService { get; init; }
    private IImageService _imageService { get; init; }
    private User _user { get; init; }

    public GetPostCommentsHandler(
        IRepository<PostEntity> postRepository,
        IPaginationService<CommentEntity> paginationService,
        IImageService imageService,
        IContextUserService contextUserService)
    {
        _postRepository = postRepository;   
        _paginationService = paginationService;
        _imageService = imageService;
        _user = contextUserService.User;
    }

    public async Task<PaginationResponse<CommentResponse>> Handle(GetPostCommentsRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<CommentResponse>();

        if (await _postRepository.GetByIdAsync(request.PostId) == null)
        {
            throw new ValidationException("Post not found");
        }

        Expression<Func<CommentEntity, bool>> predicate = item => item.PostId == request.PostId;

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.CreationTime)
            .Include(e => e.User)
                .ThenInclude(e => e.UserImage)
            .Include(e => e.CommentLikes)
            .Select(item => new CommentResponse
            {
                CommentId = item.Id,
                TextContent = item.TextContent,
                LikesCount = item.CommentLikes.Count(),
                CreatedDate = item.CreationTime,
                IsLiked = item.CommentLikes.Any(like => like.UserId == _user.Id),
                UserPreview = new UserPreviewResponse
                {
                    Id = item.UserId,
                    Login = item.User.Login,
                    Name = item.User.Name,
                    Email = item.User.Email,
                    ImageKey = item.User.UserImage.ImageKey,
                }
            })
            .ToListAsync();

        if (response.Items.Count == 0)
        {
            throw new NoContentException("No more comments left");
        }

        foreach (var post in response.Items)
        {
            if (post.UserPreview?.ImageKey != null)
            {
                post.UserPreview.ImageUrl = _imageService.GetImageUrl(post.UserPreview.ImageKey);
            }
        }

        response.TotalCount = request.Page == PaginationConsts.DefaultPage
            ? await _paginationService.GetCountAsync(predicate)
            : -1;

        response.NextLink = response.Items.Count == PaginationConsts.ItemsPerPage
            ? _paginationService.GetNextLink(request.Page, request.PostId.ToString())
            : String.Empty;

        return response;
    }
}
