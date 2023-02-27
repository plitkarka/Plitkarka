using System.Linq.Expressions;

namespace Plitkarka.Infrastructure.Services;

public interface IRepository<T>
{
    Task<Guid> AddAsync(T user);
    Task DeleteAsync(T user);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> UpdateAsync(T user);
}
