using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class CommentRepository : IRepository<CommentEntity>
{
    private MySqlDbContext _db { get; init; }

    public CommentRepository(
        MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(CommentEntity item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            var res = await _db.Comments.AddAsync(item);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task DeleteAsync(CommentEntity item)
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

    public IQueryable<CommentEntity> GetAll(bool includeNonActive = false)
    {
        return includeNonActive
            ? _db.Comments
            : _db.Comments.Where(u => u.IsActive);
    }

    public async Task<CommentEntity?> GetByIdAsync(Guid id, bool includeNonActive = false)
    {
        try
        {
            var result = includeNonActive
                ? await _db.Comments
                    .FirstOrDefaultAsync(u => u.Id == id)
                : await _db.Comments
                    .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return result;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<CommentEntity> UpdateAsync(CommentEntity item)
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
