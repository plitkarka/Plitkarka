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
    private ILogger<UserRepository> _logger;

    public RefreshTokenRepository(
        MySqlDbContext db,
        ILogger<UserRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(RefreshTokenEntity user)
    {
        if (user == null)
        {
            _logger.LogArgumentNullError(nameof(AddAsync), nameof(user));
            throw new ArgumentNullException(nameof(user));
        }

        var res = await _db.RefreshTokens.AddAsync(user);
        await _db.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task DeleteAsync(RefreshTokenEntity user)
    {
        if (user == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(user));
            throw new ArgumentNullException(nameof(user));
        }

        user.IsActive = false;

        _db.Update(user);
        await _db.SaveChangesAsync();
    }

    public async Task<RefreshTokenEntity?> GetAsync(Expression<Func<RefreshTokenEntity, bool>> predicate)
    {
        if (predicate == null)
        {
            _logger.LogArgumentNullError(nameof(GetAsync), nameof(predicate));
            throw new ArgumentNullException(nameof(predicate));
        }

        var result = await _db.RefreshTokens.FirstOrDefaultAsync(predicate);

        return result;
    }

    public async Task<RefreshTokenEntity?> GetByIdAsync(Guid id)
    {
        var result = await _db.RefreshTokens.FindAsync(id);

        return result;
    }

    public async Task<RefreshTokenEntity> UpdateAsync(RefreshTokenEntity user)
    {
        if (user == null)
        {
            _logger.LogArgumentNullError(nameof(UpdateAsync), nameof(user));
            throw new ArgumentNullException(nameof(user));
        }

        _db.Entry(user).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return user;
    }
}
