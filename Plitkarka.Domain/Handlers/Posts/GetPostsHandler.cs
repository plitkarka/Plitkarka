﻿using System.Linq.Expressions;
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
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Posts;

public class GetPostsHandler : IRequestHandler<GetPostsRequest, PaginationResponse<PostResponse>>
{
    private User _user { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }
    private IPaginationService<PostEntity> _paginationService { get; init; }
    private IImageService _imageService { get; init; }

    public GetPostsHandler(
        IContextUserService contextUserService,
        IRepository<UserEntity> userRepository,
        IPaginationService<PostEntity> paginationService,
        IImageService imageService)
    {
        _user = contextUserService.User;
        _userRepository = userRepository;
        _paginationService = paginationService;
        _imageService = imageService;
    }

    public async Task<PaginationResponse<PostResponse>> Handle(GetPostsRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<PostResponse>();

        var userId = request.UserId == Guid.Empty
            ? _user.Id
            : request.UserId;

        if (await _userRepository.GetByIdAsync(userId) == null)
        {
            throw new ValidationException("User not found");
        }

        Expression<Func<PostEntity, bool>> predicate = item => item.UserId == userId;

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
