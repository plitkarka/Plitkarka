using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class RefreshTokenRepository : IRepository<RefreshTokenEntity>
{
    private MySqlDbContext _db { get; init; }
    private ILogger<RefreshTokenRepository> _logger;

    public RefreshTokenRepository(
        MySqlDbContext db,
        ILogger<RefreshTokenRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(RefreshTokenEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(AddAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        var res = await _db.RefreshTokens.AddAsync(item);
        await _db.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task DeleteAsync(RefreshTokenEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        item.IsActive = false;

        _db.Update(item);
        await _db.SaveChangesAsync();
    }

    public IQueryable<RefreshTokenEntity> GetAll()
    {
        return _db.RefreshTokens;
    }

    public async Task<RefreshTokenEntity?> GetByIdAsync(Guid id)
    {
        var result = await _db.RefreshTokens.FindAsync(id);

        return result;
    }

    public async Task<RefreshTokenEntity> UpdateAsync(RefreshTokenEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(UpdateAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return item;
    }
}
