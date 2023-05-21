using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Domain.Handlers.Users;

public class SearchUsersHandler : IRequestHandler<SearchUsersRequest, PaginationResponse<UserPreviewResponse>>
{
    private IPaginationService<UserEntity> _paginationService { get; init; }

    public SearchUsersHandler(
        IPaginationService<UserEntity> paginationService)
    {
        _paginationService = paginationService;
    }

    public async Task<PaginationResponse<UserPreviewResponse>> Handle(SearchUsersRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<UserPreviewResponse>();

        Expression<Func<UserEntity, bool>> predicate = user => 
            user.Login.Contains(request.Filter) || user.Name.Contains(request.Filter);

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.Login)
            .Select(user => new UserPreviewResponse
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Email = user.Email
            })
            .ToListAsync();

        if (response.Items.Count == 0)
        {
            throw new NoContentException("No more users left");
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
