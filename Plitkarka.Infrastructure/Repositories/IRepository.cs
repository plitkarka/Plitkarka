using System.Linq.Expressions;

namespace Plitkarka.Infrastructure.Repositories;

public interface IRepository<T>
{
    Task<Guid> AddUserAsync(T user);
    Task DeleteUserAsync(T user);
    Task<T?> GetUserAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetUserByIdAsync(Guid id);
    Task<T> UpdateUserAsync(T user);
}
