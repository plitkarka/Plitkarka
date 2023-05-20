using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Infrastructure.Repositories;

public class UserImageRepository : IRepository<UserImageEntity>
{
    private MySqlDbContext _db { get; init; }

    public UserImageRepository(
       MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(UserImageEntity image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        try
        {
            var res = await _db.UserImages.AddAsync(image);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<UserImageEntity> GetAll() 
    {
        return _db.UserImages;
    }

    public async Task<UserImageEntity?> GetByIdAsync(Guid id)
    {
        try 
        {
            var res = await _db.UserImages.FindAsync(id);

            return res;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<UserImageEntity> UpdateAsync(UserImageEntity image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        try 
        {
            _db.Entry(image).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return image;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }
    public async Task DeleteAsync(UserImageEntity image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        try 
        { 
            image.IsActive = false;

            _db.Update(image);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

    }
}
