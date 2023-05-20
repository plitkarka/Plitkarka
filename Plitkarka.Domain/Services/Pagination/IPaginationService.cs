using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Domain.Services.Pagination;

public interface IPaginationService<T> where T : Entity
{
    Task<int> GetCountAsync();
    string GetNextLink(int page, string filter = "");
    IQueryable<T> GetPaginatedItems(int page);
}