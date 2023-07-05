using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PinedPosts;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.PinedPosts;

public class GetPinnedPostsHandler : IRequestHandler<GetPinnedPostsRequest, PaginationResponse<PostResponse>>
{
    private User _user { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }
    private IPaginationService<PostPinEntity> _paginationService { get; init; }
    private IImageService _imageService { get; init; }

    public GetPinnedPostsHandler(
        IContextUserService contextUserService,
        IRepository<UserEntity> userRepository,
        IPaginationService<PostPinEntity> paginationService,
        IImageService imageService)
{
        _user = contextUserService.User;
        _userRepository = userRepository;
        _paginationService = paginationService;
        _imageService = imageService;
    }

    public async Task<PaginationResponse<PostResponse>> Handle(GetPinnedPostsRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<PostResponse>();

        var userId = request.UserId == Guid.Empty
            ? _user.Id
            : request.UserId;

        if (await _userRepository.GetByIdAsync(userId) == null)
        {
            throw new ValidationException("User not found");
        }

        Expression<Func<PostPinEntity, bool>> predicate = item =>
            item.UserId == userId;

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.CreationTime)
            .Include(e => e.Post)
                .ThenInclude(e => e.PostImage)
            .Include(e => e.Post)
                .ThenInclude(e => e.PostLikes)
            .Include(e => e.Post)
                .ThenInclude(e => e.Comments)
            .Include(e => e.Post)
                .ThenInclude(e => e.Pins)
            .Include(e => e.Post)
                .ThenInclude(e => e.Shares)
            .Include(e => e.Post)
                .ThenInclude(e => e.User)
                    .ThenInclude(e => e.UserImage)
            .Select(item => new PostResponse
            {
                Id = item.PostId,
                TextContent = item.Post.TextContent,
                ImageKey = item.Post.PostImage.ImageKey,
                LikesCount = item.Post.PostLikes.Count(),
                CommentsCount = item.Post.Comments.Count(),
                PinsCount = item.Post.Pins.Count(),
                SharesCount = item.Post.Shares.Count(),
                CreatedDate = item.Post.CreationTime,
                UserPreview = new UserPreviewResponse
                {
                    Id = item.Post.UserId,
                    Login = item.Post.User.Login,
                    Name = item.Post.User.Name,
                    Email = item.Post.User.Email,
                    ImageKey = item.Post.User.UserImage.ImageKey,
                }
            })
            .ToListAsync();

        if (response.Items.Count == 0)
        {
            throw new NoContentException("No more posts left");
        }

        foreach (var post in response.Items)
        {
            if (post.ImageKey != null)
            {
                post.ImageUrl = _imageService.GetImageUrl(post.ImageKey);
            }

            if (post.UserPreview?.ImageKey != null)
            {
                post.UserPreview.ImageUrl = _imageService.GetImageUrl(post.UserPreview.ImageKey);
            }
        }

        response.TotalCount = request.Page == PaginationConsts.DefaultPage
            ? await _paginationService.GetCountAsync(predicate)
            : -1;

        response.NextLink = response.Items.Count == PaginationConsts.ItemsPerPage
            ? request.UserId == Guid.Empty
                ? _paginationService.GetNextLink(request.Page)
                : _paginationService.GetNextLink(request.Page, request.UserId.ToString())
            : String.Empty;

        return response;
    }
}
