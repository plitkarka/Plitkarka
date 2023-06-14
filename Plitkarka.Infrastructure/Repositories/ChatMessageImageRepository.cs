using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class ChatMessageImageRepository : IRepository<ChatMessageImageEntity>
{
    private MySqlDbContext _db { get; init; }

    public ChatMessageImageRepository(
       MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(ChatMessageImageEntity image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        try
        {
            var res = await _db.ChatMessageImages.AddAsync(image);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<ChatMessageImageEntity> GetAll(bool includeNonActive = false)
    {
        return includeNonActive
            ? _db.ChatMessageImages
            : _db.ChatMessageImages.Where(u => u.IsActive);
    }

    public async Task<ChatMessageImageEntity?> GetByIdAsync(Guid id, bool includeNonActive = false)
    {
        try
        {
            var res = includeNonActive
                ? await _db.ChatMessageImages
                    .FirstOrDefaultAsync(u => u.Id == id)
                : await _db.ChatMessageImages
                    .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return res;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<ChatMessageImageEntity> UpdateAsync(ChatMessageImageEntity image)
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
    public async Task DeleteAsync(ChatMessageImageEntity image)
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
