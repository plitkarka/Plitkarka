using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Posts;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Domain.Services.QueryablePagination;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Posts;

public class GetFeedHandler : IRequestHandler<GetFeedRequest, PaginationResponse<PostResponse>>
{
    private User _user { get; init; }
    private IQueryablePaginationService _paginationService { get; init; }
    private IRepository<SubscriptionEntity> _subscriptionRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }
    private IImageService _imageService { get; init; }

    public GetFeedHandler(
        IContextUserService contextUserService,
        IQueryablePaginationService paginationService,
        IRepository<SubscriptionEntity> subscriptionRepository,
        IRepository<PostEntity> postRepository,
        IImageService imageService)
    {
        _user = contextUserService.User;
        _subscriptionRepository = subscriptionRepository;
        _paginationService = paginationService;
        _postRepository = postRepository;
        _imageService = imageService;
    }

    public async Task<PaginationResponse<PostResponse>> Handle(GetFeedRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<PostResponse>();

        var query = _postRepository
            .GetAll()
            .Join(
                _subscriptionRepository.GetAll(),
                post => post.UserId,
                subscription => subscription.SubscribedToId,
                (post, subscription) => new
                {
                    Post = post,
                    Subscription = subscription
                })
            .Where(e => e.Subscription.UserId == _user.Id)
            .Select(e => e.Post)
            .OrderByDescending(e => e.CreationTime);

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(query, request.Page)
            .Include(e => e.User)
                .ThenInclude(e => e.UserImage)
            .Include(e => e.PostImage)
            .Include(e => e.PostLikes)
            .Include(e => e.Comments)
            .Include(e => e.Pins)
            .Include(e => e.Shares)
            .Select(item => new PostResponse
            {
                Id = item.Id,
                TextContent = item.TextContent,
                ImageKey = item.PostImage.ImageKey,
                LikesCount = item.PostLikes.Count(),
                CommentsCount = item.Comments.Count(),
                PinsCount = item.Pins.Count(),
                SharesCount = item.Shares.Count(),
                CreatedDate = item.CreationTime,
                IsLiked = item.PostLikes.Any(like => like.UserId == _user.Id),
                IsShared = item.Shares.Any(share => share.UserId == _user.Id),
                IsPinned = item.Pins.Any(pin => pin.UserId == _user.Id),
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
            ? await _paginationService.GetCountAsync(query)
            : -1;

        response.NextLink = response.Items.Count == PaginationConsts.ItemsPerPage
            ? _paginationService.GetNextLink(request.Page)
            : String.Empty;

        return response;
    }
}
