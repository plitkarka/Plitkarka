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

namespace Plitkarka.Domain.Handlers.Subscriptions;

public class GetSubscriptionsHandler : IRequestHandler<GetSubscriptionsRequest, PaginationResponse<UserPreviewResponse>>
{
    private User _user { get; init; }
    private IPaginationService<SubscriptionEntity> _paginationService { get; init; }
    private IImageService _imageService { get; init; }

    public GetSubscriptionsHandler(
        IContextUserService contextUserService,
        IPaginationService<SubscriptionEntity> paginationService,
        IImageService imageService)
    {
        _user = contextUserService.User;
        _paginationService = paginationService;
        _imageService = imageService;
    }

    public async Task<PaginationResponse<UserPreviewResponse>> Handle(GetSubscriptionsRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<UserPreviewResponse>();

        var userId = request.UserId == Guid.Empty
            ? _user.Id
            : request.UserId;

        if (!await _paginationService.IsEntityExists(userId))
        {
            throw new ValidationException("User not found");
        }

        Expression<Func<SubscriptionEntity, bool>> predicate = sub =>
            sub.IsActive && sub.UserId == userId;

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.CreationTime)
            .Include(e => e.User)
                .ThenInclude(e => e.UserImage)
            .Select(item => new UserPreviewResponse
            {
                UserId = item.SubscribedToId,
                Login = item.SubscribedTo.Login,
                Name = item.SubscribedTo.Name,
                Email = item.SubscribedTo.Email,
                ImageKey = item.User.UserImage.ImageKey
            })
            .ToListAsync();

        if (response.Items.Count == 0)
        {
            throw new NoContentException("No more subscriptions left");
        }

        foreach (var user in response.Items)
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
