using System.Linq.Expressions;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Domain.Services.Pagination
{
    public interface IPaginationService<T> where T : Entity
    {
        Task<int> GetCountAsync(Expression<Func<T, bool>>? predicate = null);
        string GetNextLink(int page, string filter = "");
        IQueryable<T> GetPaginatedItemsQuery(int page, Expression<Func<T, bool>>? where = null);
        IQueryable<T> GetPaginatedItemsQuery<TOrderKey>(
            int page,
            Expression<Func<T, bool>>? where = null,
            Expression<Func<T, TOrderKey>>? orderBy = null);
    }
}