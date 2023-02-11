using MediatR;
using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Microsoft.Extensions.Logging;

namespace Plitkarka.Domain.Handlers.Users;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, User>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger _logger { get; init; }

    public UpdateUserHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILoggerFactory loggerFactory)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdateUserHandler>();
    }

    public async Task<User> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var toUpdate = request.ToUpdate;

        try
        {
            var existingUser = await _repository.GetUserByIdAsync(toUpdate.Id);

            if (existingUser.Email != toUpdate.Email || existingUser.Login != toUpdate.Login)
            {
                var userExist = await _repository.GetUserAsync(
                    user => (user.Email == toUpdate.Email || user.Login == toUpdate.Login) && user.Id != toUpdate.Id);

                if (userExist != null)
                {
                    if (userExist.Email == existingUser.Email)
                    {
                        throw new ValidationException("This Email is already used", nameof(existingUser.Email));
                    }

                    if (userExist.Login == existingUser.Login)
                    {
                        throw new ValidationException("This Login is already used", nameof(existingUser.Login));
                    }
                }
            }

            var updatedUser = await _repository.UpdateUserAsync(
                _mapper.Map<UserEntity>(toUpdate));

            return _mapper.Map<User>(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(UpdateUserHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
