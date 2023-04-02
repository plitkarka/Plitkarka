﻿using MediatR;
using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Microsoft.Extensions.Logging;
using Plitkarka.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Plitkarka.Domain.Handlers.Users;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, User?>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger<UpdateUserHandler> _logger { get; init; }

    public UpdateUserHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILogger<UpdateUserHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<User?> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var toUpdate = request.ToUpdate;
        bool changes = false;

        try
        {
            var existingUser = await _repository.GetByIdAsync(toUpdate.Id);

            if (existingUser == null)
            {
                throw new ValidationException("Failed to update user information. User does not exist");
            }

            // check if FirstName is not null and changed
            if (toUpdate.FirstName != null && existingUser.Name != toUpdate.FirstName)
            {
                existingUser.Name = toUpdate.FirstName;
                changes = true;
            }

            // check if BirthDate is not default and changed
            if (toUpdate.BirthDate != default && existingUser.BirthDate != toUpdate.BirthDate)
            {
                existingUser.BirthDate = toUpdate.BirthDate;
                changes = true;
            }

            // check if Email is not null and changed
            if (toUpdate.Email != null && existingUser.Email != toUpdate.Email)
            {
                var userExist = await _repository.GetAll().FirstOrDefaultAsync(
                    user => user.Email == toUpdate.Email && user.Id != toUpdate.Id);

                existingUser.Email = userExist != null && toUpdate.Email == userExist.Email
                    ? throw new ValidationException("This Email is already used", nameof(existingUser.Email))
                    : toUpdate.Email;

                changes = true;
            }

            // check if Login is not null and changed
            if (toUpdate.Login != null && existingUser.Login != toUpdate.Login)
            {
                var userExist = await _repository.GetAll().FirstOrDefaultAsync(
                    user => user.Login == toUpdate.Login && user.Id != toUpdate.Id);

                existingUser.Login = userExist != null && toUpdate.Login == userExist.Login 
                    ? throw new ValidationException("This Login is already used", nameof(existingUser.Login))
                    : toUpdate.Login;

                changes = true;
            }

            if (!changes)
            {
                return null;
            }

            var updatedUser = await _repository.UpdateAsync(existingUser);

            return _mapper.Map<User>(updatedUser);
        }
        catch (Exception ex) when (ex is not ValidationException)
        {
            _logger.LogDatabaseError($"{nameof(UpdateUserHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
