using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Infrastructure.Models.Abstractions;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Services.Pagination;

public class PaginationService<T> : IPaginationService<T> where T : Entity
{
    private IRepository<T> _repository { get; init; }

    public PaginationService(
        IRepository<T> repository)
    {
        _repository = repository;
    }

    public IQueryable<T> GetPaginatedItemsQuery(
        int page,
        Expression<Func<T, bool>>? where = null)
    {
        return GetPaginatedItemsQuery(
            page,
            where,
            orderBy: e => e.Id);
    }

    public IQueryable<T> GetPaginatedItemsQuery<TOrderKey>(
        int page,
        Expression<Func<T, bool>>? where = null,
        Expression<Func<T, TOrderKey>>? orderBy = null)
    {
        var query = _repository.GetAll();

        if (orderBy != null)
        {
            query = query
                .OrderByDescending(orderBy);
        }

        if (where != null)
        {
            query = query
                .Where(where);
        }

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

    public async Task<int> GetCountAsync(
        Expression<Func<T, bool>>? predicate = null)
    {
        var query = _repository.GetAll();

        if (predicate != null)
    {
            query = query
                .Where(predicate);
        }

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
    
    public async Task<bool> IsEntityExists(Guid id)
    {
        return await _repository.GetByIdAsync(id) != null;
    }

    private string GetExceptionMessage(Exception ex)
        => $"PaginationService throws exception: {ex.Message}";
}
