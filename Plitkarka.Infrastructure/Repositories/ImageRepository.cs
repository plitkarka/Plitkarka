using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Services;

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

        var res = await _db.Images.AddAsync(image);
        await _db.SaveChangesAsync();

        return res.Entity.Id;
    }

    public IQueryable<ImageEntity> GetAll() 
    {
        return _db.Images;
    }

    public async Task<ImageEntity?> GetByIdAsync(Guid id)
    {
        var res = await _db.Images.FindAsync(id);

        return res;
    }

    public async Task<ImageEntity> UpdateAsync(ImageEntity image)
    {
        if (image == null)
        {
            _logger.LogArgumentNullError(nameof(UpdateAsync), nameof(image));
            throw new ArgumentNullException(nameof(image));
        }

        _db.Entry(image).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return image;
    }
    public async Task DeleteAsync(ImageEntity image)
    {
        if (image == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(image));
            throw new ArgumentNullException(nameof(image));
        }

        image.IsActive = false;

        _db.Update(image);
        await _db.SaveChangesAsync();
    }
}
