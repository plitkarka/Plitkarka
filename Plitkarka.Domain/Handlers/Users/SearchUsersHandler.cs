using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Domain.Services.Pagination;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Domain.Handlers.Users;

public class SearchUsersHandler : IRequestHandler<SearchUsersRequest, PaginationResponse<UserPreviewResponse>>
{
    private IPaginationService<UserEntity> _paginationService { get; init; }
    private IImageService _imageService { get; init; }

    public SearchUsersHandler(
        IPaginationService<UserEntity> paginationService,
        IImageService imageService)
    {
        _paginationService = paginationService;
        _imageService = imageService;
    }

    public async Task<PaginationResponse<UserPreviewResponse>> Handle(SearchUsersRequest request, CancellationToken cancellationToken)
    {
        var response = new PaginationResponse<UserPreviewResponse>();

        Expression<Func<UserEntity, bool>> predicate = user => 
            user.IsActive && (user.Login.Contains(request.Filter) || user.Name.Contains(request.Filter));

        response.Items = await _paginationService
            .GetPaginatedItemsQuery(
                request.Page,
                where: predicate,
                orderBy: e => e.Login)
            .Include(e => e.UserImage)
            .Select(item => new UserPreviewResponse
            {
                Id = item.Id,
                Login = item.Login,
                Name = item.Name,
                Email = item.Email,
                ImageKey = item.UserImage.ImageKey
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
