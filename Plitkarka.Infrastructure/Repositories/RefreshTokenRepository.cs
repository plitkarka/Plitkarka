using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
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

        try
        {
            var res = await _db.RefreshTokens.AddAsync(item);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(RefreshTokenRepository)}.{nameof(AddAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(RefreshTokenEntity item)
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
            _logger.LogDatabaseError($"{nameof(RefreshTokenRepository)}.{nameof(DeleteAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<RefreshTokenEntity> GetAll()
    {
        return _db.RefreshTokens;
    }

    public async Task<RefreshTokenEntity?> GetByIdAsync(Guid id)
    {
        try 
        { 
            var result = await _db.RefreshTokens.FindAsync(id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(RefreshTokenRepository)}.{nameof(GetByIdAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<RefreshTokenEntity> UpdateAsync(RefreshTokenEntity item)
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
            _logger.LogDatabaseError($"{nameof(RefreshTokenRepository)}.{nameof(UpdateAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
