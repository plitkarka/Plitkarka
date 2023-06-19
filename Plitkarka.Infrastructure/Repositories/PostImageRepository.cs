using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Infrastructure.Repositories;

public class PostImageRepository : IRepository<PostImageEntity>
{
    private MySqlDbContext _db { get; init; }

    public PostImageRepository(
       MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(PostImageEntity image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        try
        {
            var res = await _db.PostImages.AddAsync(image);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<PostImageEntity> GetAll(bool includeNonActive = false) 
    {
        return includeNonActive
            ? _db.PostImages
            : _db.PostImages.Where(u => u.IsActive);
    }

    public async Task<PostImageEntity?> GetByIdAsync(Guid id, bool includeNonActive = false)
    {
        try 
        {
            var res = includeNonActive
                ? await _db.PostImages
                    .FirstOrDefaultAsync(u => u.Id == id)
                : await _db.PostImages
                    .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return res;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<PostImageEntity> UpdateAsync(PostImageEntity image)
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
    public async Task DeleteAsync(PostImageEntity image)
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
