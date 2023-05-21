using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Subscriptions;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Domain.Handlers.Subscriptions;

public class GetSubscriptionsHandler : IRequestHandler<GetSubscriptionsRequest, PaginationResponse<UserPreviewResponse>>
{
    private User _user { get; init; }

    private IPaginationService<SubscriptionEntity> _paginationService { get; init; }

    public GetSubscriptionsHandler(
        IContextUserService contextUserService,
        IPaginationService<SubscriptionEntity> paginationService)
    {
        _user = contextUserService.User;
        _paginationService = paginationService;
    }

    public async Task<PaginationResponse<UserPreviewResponse>> Handle(GetSubscriptionsRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<UserPreviewResponse>();

        var userId = request.UserId == Guid.Empty
            ? _user.Id
            : request.UserId;

        Expression<Func<SubscriptionEntity, bool>> predicate = sub =>
            sub.UserId == userId;

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.CreationTime)
            .Include(e => e.SubscribedTo)
            .Select(subscription => new UserPreviewResponse
            {
                Id = subscription.SubscribedToId,
                Login = subscription.SubscribedTo.Login,
                Name = subscription.SubscribedTo.Name,
                Email = subscription.SubscribedTo.Email
            })
            .ToListAsync();

        if (response.Items.Count == 0)
        {
            throw new NoContentException("No more subscriptions left");
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
