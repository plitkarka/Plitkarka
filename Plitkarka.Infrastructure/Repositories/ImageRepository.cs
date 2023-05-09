using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Infrastructure.Repositories;

public class ImageRepository : IRepository<ImageEntity>
{
    private MySqlDbContext _db { get; init; }

    public ImageRepository(
       MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(ImageEntity image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        try
        {
            var res = await _db.Images.AddAsync(image);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<ImageEntity> GetAll() 
    {
        return _db.Images;
    }

    public async Task<ImageEntity?> GetByIdAsync(Guid id)
    {
        try 
        {
            var res = await _db.Images.FindAsync(id);

            return res;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<ImageEntity> UpdateAsync(ImageEntity image)
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
    public async Task DeleteAsync(ImageEntity image)
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
