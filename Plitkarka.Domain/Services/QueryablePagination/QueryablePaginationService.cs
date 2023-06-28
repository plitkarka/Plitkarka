using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;

namespace Plitkarka.Domain.Services.QueryablePagination;

public class QueryablePaginationService : IQueryablePaginationService
{
    public IQueryable<T> GetPaginatedItemsQuery<T>(
        IQueryable<T> query,
        int page)
    {
        try
        {
            return query
                .Skip(page * PaginationConsts.ItemsPerPage)
                .Take(PaginationConsts.ItemsPerPage);
        }
        catch (Exception ex)
        {
            throw new MySqlException(GetExceptionMessage(ex));
        }
    }

    public async Task<int> GetCountAsync<T>(
        IQueryable<T> query)
    {
        try
        {
            return await query.CountAsync();
        }
        catch (Exception ex)
        {
            throw new MySqlException(GetExceptionMessage(ex));
        }
    }

    public string GetNextLink(int page, string filter = "")
    {
        var nextLink = $"?Page={page + 1}";

        return filter.IsNullOrEmpty()
            ? nextLink
            : $"{nextLink}&Filter={filter}";
    }

    private string GetExceptionMessage(Exception ex)
        => $"QueryablePaginationService throws exception: {ex.Message}";
}
