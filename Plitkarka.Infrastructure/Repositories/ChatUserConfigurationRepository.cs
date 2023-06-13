﻿using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Infrastructure.Repositories;

public class ChatUserConfigurationRepository : IRepository<ChatUserConfigurationEntity>
{
    private MySqlDbContext _db { get; init; }

    public ChatUserConfigurationRepository(
        MySqlDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> AddAsync(ChatUserConfigurationEntity item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        try
        {
            var res = await _db.ChatUserConfigurations.AddAsync(item);
            await _db.SaveChangesAsync();

            return res.Entity.Id;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public async Task<ChatUserConfigurationEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _db.ChatUserConfigurations
                .FirstOrDefaultAsync(u => u.Id == id);

            return result;
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }
    }

    public IQueryable<ChatUserConfigurationEntity> GetAll()
    {
        return _db.ChatUserConfigurations;
    }

    public async Task<ChatUserConfigurationEntity> UpdateAsync(ChatUserConfigurationEntity item)
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

    public async Task DeleteAsync(ChatUserConfigurationEntity item)
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
