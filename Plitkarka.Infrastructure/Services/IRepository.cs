using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Services;

public interface IRepository<T> where T : Entity
{
    Task<Guid> AddAsync(T item);
    Task DeleteAsync(T item);
    IQueryable<T> GetAll();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> UpdateAsync(T item);
}
