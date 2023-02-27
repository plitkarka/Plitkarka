using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class UserRepository : IRepository<UserEntity>
{
    private MySqlDbContext _db { get; init; }
    private ILogger<UserRepository> _logger;

    public UserRepository(
        MySqlDbContext db,
        ILogger<UserRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(UserEntity user)
    {
        if (user == null)
        {
            _logger.LogArgumentNullError(nameof(AddAsync), nameof(user));
            throw new ArgumentNullException(nameof(user));
        }

        var res = await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return res.Entity.Id;
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        UserEntity? result = await _db.Users.FindAsync(id);

        return result;
    }

    public async Task<UserEntity?> GetAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        if (predicate == null)
        {
            _logger.LogArgumentNullError(nameof(GetAsync), nameof(predicate));
            throw new ArgumentNullException(nameof(predicate));
        }

        UserEntity? result = await _db.Users.FirstOrDefaultAsync(predicate);

        return result;
    }

    public async Task<UserEntity> UpdateAsync(UserEntity user)
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

    public async Task DeleteAsync(UserEntity user)
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
}
