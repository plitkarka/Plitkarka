﻿using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class ChatMessageRepository : IRepository<ChatMessageEntity>
{
    private MySqlDbContext _db { get; init; }

    public ChatMessageRepository(
        MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(ChatMessageEntity item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            var res = await _db.ChatMessages.AddAsync(item);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<ChatMessageEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _db.ChatMessages
                .FirstOrDefaultAsync(u => u.Id == id);

            return result;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<ChatMessageEntity> GetAll()
    {
        return _db.ChatMessages;
    }

    public async Task<ChatMessageEntity> UpdateAsync(ChatMessageEntity item)
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

    public async Task DeleteAsync(ChatMessageEntity item)
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
}
