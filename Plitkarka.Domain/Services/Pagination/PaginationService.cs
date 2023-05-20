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

    public IQueryable<T> GetPaginatedItems(int page)
    {
        try
        {
            return _repository.GetAll()
                .Skip(page * PaginationConsts.ItemsPerPage)
                .Take(PaginationConsts.ItemsPerPage);
        }
        catch (Exception ex)
        {
            throw new MySqlException(GetExceptionMessage(ex));
        }
    }

    public async Task<int> GetCountAsync()
    {
        try
        {
            return await _repository.GetAll().CountAsync();
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
        => $"PaginationService throws exception: {ex.Message}";
}
