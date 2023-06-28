using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class SubscriptionRepository : IRepository<SubscriptionEntity>
{
    private MySqlDbContext _db { get; init; }

    public SubscriptionRepository(
        MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(SubscriptionEntity item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            var res = await _db.Subscriptions.AddAsync(item);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(SubscriptionEntity item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            item.IsActive = false;

            _db.Update(item);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<SubscriptionEntity> GetAll(bool includeNonActive = false)
    {
        return includeNonActive
            ? _db.Subscriptions
            : _db.Subscriptions.Where(u => u.IsActive);
    }

    public async Task<SubscriptionEntity?> GetByIdAsync(Guid id, bool includeNonActive = false)
    {
        try
        {
            var result = includeNonActive
                ? await _db.Subscriptions
                    .FirstOrDefaultAsync(u => u.Id == id)
                : await _db.Subscriptions
                    .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return result;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<SubscriptionEntity> UpdateAsync(SubscriptionEntity item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return item;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }
}
