using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Infrastructure.Repositories;

public class UserRepository : IRepository<UserEntity>
{
    private MySqlDbContext _db { get; init; }

    public UserRepository(
        MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(UserEntity user)
    {
        if (user == null)
        {
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
            throw new MySqlException(ex.Message);
        }        
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id, bool includeNonActive = false)
    {
        try 
        {
            var result = includeNonActive 
                ? await _db.Users
                    .FirstOrDefaultAsync(u => u.Id == id)
                : await _db.Users
                    .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return result;
        }
        catch(Exception ex)
        {
            throw new MySqlException(ex.Message);
        }      
    }

    public IQueryable<UserEntity> GetAll(bool includeNonActive = false)
    {
        return includeNonActive
            ? _db.Users
            : _db.Users.Where(u => u.IsActive);
    }

    public async Task<UserEntity> UpdateAsync(UserEntity user)
    {
        if (user == null)
        {
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
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(UserEntity user)
    {
        if (user == null)
        {
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
            throw new MySqlException(ex.Message);
        }
    }
}
