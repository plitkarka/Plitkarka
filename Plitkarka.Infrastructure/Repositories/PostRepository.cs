using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class PostRepository : IRepository<PostEntity>
{
    private MySqlDbContext _db { get; init; }

    public PostRepository(
        MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(PostEntity item)
    {
        if (item == null)
        {
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
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(PostEntity item)
    {
        if (item == null)
        {
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
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<PostEntity> GetAll(bool includeNonActive = false)
    {
        return includeNonActive
            ? _db.Posts
            : _db.Posts.Where(u => u.IsActive);
    }

    public async Task<PostEntity?> GetByIdAsync(Guid id, bool includeNonActive = false)
    {
        try
        {
            var result = includeNonActive
                ? await _db.Posts
                    .FirstOrDefaultAsync(u => u.Id == id)
                : await _db.Posts
                    .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return result;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<PostEntity> UpdateAsync(PostEntity item)
    {
        if (item == null)
        {
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
            throw new MySqlException(ex.Message);
        }
    }
}
