using MediatR;
using AutoMapper;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Microsoft.Extensions.Logging;

namespace Plitkarka.Domain.Handlers.Users;

public class AddUserHandler : IRequestHandler<AddUserRequest, Guid>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger _logger { get; init; }

    public AddUserHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILoggerFactory loggerFactory)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<AddUserHandler>();
    }

    public async Task<Guid> Handle(AddUserRequest request, CancellationToken cancellationToken)
    {
        var newUser = request.NewUser;
        UserEntity? userExist;

        try
        {
            userExist = await _repository.GetUserAsync(
                user => user.Email == newUser.Email || user.Login == newUser.Login);

            if (userExist != null)
            {
                var id = await _repository.AddUserAsync(
                    _mapper.Map<UserEntity>(newUser));

                return id;
            }
        }
        catch(Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(AddUserHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
        
        if (userExist.Email == newUser.Email)
        {
            throw new ValidationException("This Email is already used", nameof(newUser.Email));
        }

        if (userExist.Login == newUser.Login)
        {
            throw new ValidationException("This Login is already used", nameof(newUser.Login));
        }

        throw new ValidationException("New user data is invalid"); 
    }
}
