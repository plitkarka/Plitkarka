using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class RefreshTokenRepository : IRepository<RefreshTokenEntity>
{
    private MySqlDbContext _db { get; init; }

    public RefreshTokenRepository(
        MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(RefreshTokenEntity item)
    {
        if (item == null)
        {
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
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(RefreshTokenEntity item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        try 
        { 
            _db.RefreshTokens.Remove(item);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<RefreshTokenEntity> GetAll(bool includeNonActive = false)
    {
        return _db.RefreshTokens;
    }

    public async Task<RefreshTokenEntity?> GetByIdAsync(Guid id, bool includeNonActive = false)
    {
        try 
        { 
            var result = await _db.RefreshTokens.FindAsync(id);

            return result;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<RefreshTokenEntity> UpdateAsync(RefreshTokenEntity item)
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
