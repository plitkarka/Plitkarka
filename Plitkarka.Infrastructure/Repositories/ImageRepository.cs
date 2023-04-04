using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Infrastructure.Repositories;

public class ImageRepository : IRepository<ImageEntity>
{
    private MySqlDbContext _db { get; init; }
    private ILogger<ImageRepository> _logger { get; init; }

    public ImageRepository(
       MySqlDbContext db,
       ILogger<ImageRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(ImageEntity image)
    {
        if (image == null)
        {
            _logger.LogArgumentNullError(nameof(AddAsync), nameof(image));
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
            _logger.LogDatabaseError($"{nameof(ImageRepository)}.{nameof(AddAsync)}", ex.Message);
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
            _logger.LogDatabaseError($"{nameof(ImageRepository)}.{nameof(GetByIdAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<ImageEntity> UpdateAsync(ImageEntity image)
    {
        if (image == null)
        {
            _logger.LogArgumentNullError(nameof(UpdateAsync), nameof(image));
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
            _logger.LogDatabaseError($"{nameof(ImageRepository)}.{nameof(UpdateAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
    public async Task DeleteAsync(ImageEntity image)
    {
        if (image == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(image));
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
            _logger.LogDatabaseError($"{nameof(ImageRepository)}.{nameof(DeleteAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }

    }
}
