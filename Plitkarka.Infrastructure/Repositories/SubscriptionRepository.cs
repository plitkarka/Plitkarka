using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class SubscriptionRepository : IRepository<SubscriptionEntity>
{
    private MySqlDbContext _db { get; init; }
    private ILogger<SubscriptionRepository> _logger;

    public SubscriptionRepository(
        MySqlDbContext db,
        ILogger<SubscriptionRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(SubscriptionEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(AddAsync), nameof(item));
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
            _logger.LogDatabaseError($"{nameof(SubscriptionRepository)}.{nameof(AddAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(SubscriptionEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(item));
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
            _logger.LogDatabaseError($"{nameof(SubscriptionRepository)}.{nameof(DeleteAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<SubscriptionEntity> GetAll()
    {
        return _db.Subscriptions;
    }

    public async Task<SubscriptionEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _db.Subscriptions.FindAsync(id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(SubscriptionRepository)}.{nameof(GetByIdAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<SubscriptionEntity> UpdateAsync(SubscriptionEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(UpdateAsync), nameof(item));
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
            _logger.LogDatabaseError($"{nameof(SubscriptionRepository)}.{nameof(UpdateAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
