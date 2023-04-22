using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class CommentLikeRepository : IRepository<CommentLikeEntity>
{
    private MySqlDbContext _db { get; init; }
    private ILogger<CommentLikeRepository> _logger;

    public CommentLikeRepository(
        MySqlDbContext db,
        ILogger<CommentLikeRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(CommentLikeEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(AddAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            var res = await _db.CommentLikes.AddAsync(item);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(CommentLikeRepository)}.{nameof(AddAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(CommentLikeEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            _db.CommentLikes.Remove(item);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(CommentLikeRepository)}.{nameof(DeleteAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<CommentLikeEntity> GetAll()
    {
        return _db.CommentLikes;
    }

    public async Task<CommentLikeEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _db.CommentLikes.FindAsync(id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(CommentLikeRepository)}.{nameof(GetByIdAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<CommentLikeEntity> UpdateAsync(CommentLikeEntity item)
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
            _logger.LogDatabaseError($"{nameof(CommentLikeRepository)}.{nameof(UpdateAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
