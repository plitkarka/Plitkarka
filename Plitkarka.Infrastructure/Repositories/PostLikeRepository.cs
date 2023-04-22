using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class PostLikeRepository : IRepository<PostLikeEntity>
{
    private MySqlDbContext _db { get; init; }
    private ILogger<PostLikeRepository> _logger;

    public PostLikeRepository(
        MySqlDbContext db,
        ILogger<PostLikeRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(PostLikeEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(AddAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            var res = await _db.PostLikes.AddAsync(item);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(PostLikeRepository)}.{nameof(AddAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(PostLikeEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            _db.PostLikes.Remove(item);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(PostLikeRepository)}.{nameof(DeleteAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<PostLikeEntity> GetAll()
    {
        return _db.PostLikes;
    }

    public async Task<PostLikeEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _db.PostLikes.FindAsync(id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(PostLikeRepository)}.{nameof(GetByIdAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<PostLikeEntity> UpdateAsync(PostLikeEntity item)
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
            _logger.LogDatabaseError($"{nameof(PostLikeRepository)}.{nameof(UpdateAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
