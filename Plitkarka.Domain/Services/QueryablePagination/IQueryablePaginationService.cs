namespace Plitkarka.Domain.Services.QueryablePagination;

public interface IQueryablePaginationService
{
    Task<int> GetCountAsync<T>(IQueryable<T> query);
    string GetNextLink(int page, string filter = "");
    IQueryable<T> GetPaginatedItemsQuery<T>(IQueryable<T> query, int page);
}