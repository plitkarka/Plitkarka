using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class PostRepository : IRepository<PostEntity>
{
    private MySqlDbContext _db { get; init; }
    private ILogger<PostRepository> _logger;

    public PostRepository(
        MySqlDbContext db,
        ILogger<PostRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(PostEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(AddAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            var res = await _db.Posts.AddAsync(item);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(PostRepository)}.{nameof(AddAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(PostEntity item)
    {
        if (item == null)
        {
            _logger.LogArgumentNullError(nameof(DeleteAsync), nameof(item));
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            item.IsActive = false;

            _db.Update(item);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(PostRepository)}.{nameof(DeleteAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<PostEntity> GetAll()
    {
        return _db.Posts;
    }

    public async Task<PostEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _db.Posts.FindAsync(id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(PostRepository)}.{nameof(GetByIdAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<PostEntity> UpdateAsync(PostEntity item)
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
            _logger.LogDatabaseError($"{nameof(PostRepository)}.{nameof(UpdateAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
