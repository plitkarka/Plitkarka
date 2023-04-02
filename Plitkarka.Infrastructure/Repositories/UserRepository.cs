using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Commons.Exceptions;

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

        try
        {
            var res = await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch(Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(UserRepository)}.{nameof(AddAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }        
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        try 
        { 
            var result = await _db.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.Id == id);

            return result;
        }
        catch(Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(UserRepository)}.{nameof(GetByIdAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }      
    }

    public IQueryable<UserEntity> GetAll()
    {
        return _db.Users;
    }

    public async Task<UserEntity> UpdateAsync(UserEntity user)
    {
        if (user == null)
        {
            _logger.LogArgumentNullError(nameof(UpdateAsync), nameof(user));
            throw new ArgumentNullException(nameof(user));
        }

        try
        {
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(UserRepository)}.{nameof(UpdateAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(UserEntity user)
    {
        if (user == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(user));
            throw new ArgumentNullException(nameof(user));
        }

        try
        {
            user.IsActive = false;

            _db.Update(user);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(UserRepository)}.{nameof(DeleteAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
