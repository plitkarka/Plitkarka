using System;
using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Posts;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Posts;

public class SearchPostsHandler : IRequestHandler<SearchPostsRequest, PaginationResponse<PostResponse>>
{
    private IPaginationService<PostEntity> _paginationService { get; init; }
    private IImageService _imageService { get; init; }

    public SearchPostsHandler(
        IPaginationService<PostEntity> paginationService,
        IImageService imageService)
    {
        _paginationService = paginationService;
        _imageService = imageService;
    }

    public async Task<PaginationResponse<PostResponse>> Handle(SearchPostsRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<PostResponse>();

        if(request.Filter.IsNullOrEmpty())
        {
            throw new ValidationException("Filter is empty");
        }

        var filter = request.Filter.ToLower();

        Expression<Func<PostEntity, bool>> predicate = item => 
            item.TextContent.ToLower().Contains(filter) ||
            item.User.Login.ToLower().Contains(filter);

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.CreationTime)
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
            ? await _paginationService.GetCountAsync(predicate)
            : -1;

        response.NextLink = response.Items.Count == PaginationConsts.ItemsPerPage
            ? _paginationService.GetNextLink(request.Page, request.Filter)
            : String.Empty;

        return response;
    }
}