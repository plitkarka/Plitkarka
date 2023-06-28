using System;
using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Subscriptions;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Subscriptions;

public class GetSubscribersHandler : IRequestHandler<GetSubscribersRequest, PaginationResponse<UserPreviewResponse>>
{
    private User _user { get; init; }
    private IPaginationService<SubscriptionEntity> _paginationService { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }
    private IImageService _imageService { get; init; }

    public GetSubscribersHandler(
        IContextUserService contextUserService,
        IPaginationService<SubscriptionEntity> paginationService,
        IRepository<UserEntity> userRepository,
        IImageService imageService)
    {
        _user = contextUserService.User;
        _paginationService = paginationService;
        _userRepository = userRepository;
        _imageService = imageService;
    }

    public async Task<PaginationResponse<UserPreviewResponse>> Handle(GetSubscribersRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<UserPreviewResponse>();

        var userId = request.UserId == Guid.Empty
            ? _user.Id
            : request.UserId;

        if (await _userRepository.GetByIdAsync(userId) == null)
        {
            throw new ValidationException("User not found");
        }

        Expression<Func<SubscriptionEntity, bool>> predicate = sub => sub.SubscribedToId == userId;

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.CreationTime)
            .Include(e => e.User)
                .ThenInclude(e => e.UserImage)
            .Select(item => new UserPreviewResponse
            {
                Id = item.UserId,
                Login = item.User.Login,
                Name = item.User.Name,
                Email = item.User.Email,
                ImageKey = item.User.UserImage.ImageKey
            })
            .ToListAsync();

        if (response.Items.Count == 0)
        {
            throw new NoContentException("No more subscribers left");
        }

        foreach(var user in response.Items)
        {
            if (user.ImageKey != null)
            {
                user.ImageUrl = _imageService.GetImageUrl(user.ImageKey);
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
