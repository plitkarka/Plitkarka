using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Domain.Handlers.Users;

public class SearchUsersHandler : IRequestHandler<SearchUsersRequest, PaginationResponse<UserPreviewSubscriptionResponse>>
{
    private User _user { get; init; }
    private IPaginationService<UserEntity> _paginationService { get; init; }
    private IImageService _imageService { get; init; }

    public SearchUsersHandler(
        IContextUserService contextUserService,
        IPaginationService<UserEntity> paginationService,
        IImageService imageService)
    {
        _paginationService = paginationService;
        _imageService = imageService;
        _user = contextUserService.User;
    }

    public async Task<PaginationResponse<UserPreviewSubscriptionResponse>> Handle(SearchUsersRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<UserPreviewSubscriptionResponse>();

        var filter = request.Filter.ToLower();

        Expression<Func<UserEntity, bool>> predicate = user =>
            user.Login.ToLower().Contains(filter) ||
            user.Name.ToLower().Contains(filter) &&
            user.Id != _user.Id;

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.Login)
            .Include(e => e.UserImage)
            .Include(e => e.Subscribers)
            .Select(item => new UserPreviewSubscriptionResponse
            {
                Id = item.Id,
                Login = item.Login,
                Name = item.Name,
                Email = item.Email,
                ImageKey = item.UserImage.ImageKey,
                IsSubscribed = item.Subscribers.Any(sub => sub.UserId == _user.Id),
            })
            .ToListAsync();

        if (response.Items.Count == 0)
        {
            throw new NoContentException("No more users left");
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
            ? _paginationService.GetNextLink(request.Page, request.Filter)
            : String.Empty;

        return response;
    }
}
