using MediatR;
using AutoMapper;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Microsoft.Extensions.Logging;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Plitkarka.Domain.Handlers.Users;

public class AddUserHandler : IRequestHandler<AddUserRequest, Guid>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger<AddUserHandler> _logger { get; init; }
    private IEncryptionService _encryptionService { get; init; }

    public AddUserHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILogger<AddUserHandler> logger,
        IEncryptionService encryptionService)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _encryptionService = encryptionService;
    }

    public async Task<Guid> Handle(AddUserRequest request, CancellationToken cancellationToken)
    {
        var newUser = request.NewUser;
        UserEntity? userExist;

        try
        {
            userExist = await _repository.GetAll().FirstOrDefaultAsync(
                user => user.Email == newUser.Email || user.Login == newUser.Login);

            if (userExist != null)
            {
                if (userExist.Email == newUser.Email)
                {
                    throw new ValidationException("This Email is already used", nameof(newUser.Email));
                }

                if (userExist.Login == newUser.Login)
                {
                    throw new ValidationException("This Login is already used", nameof(newUser.Login));
                }
            }

            newUser.Salt = _encryptionService.GenerateSalt();
            newUser.Password = _encryptionService.Hash(newUser.Password + newUser.Salt);
            newUser.CreatedDate = DateTime.UtcNow.Date;
            newUser.EmailCode = "123456";
            newUser.PasswordAttempts = User.PasswordAttemptsCount;

            var id = await _repository.AddAsync(
                _mapper.Map<UserEntity>(newUser));

            return id;
        }
        catch(Exception ex) when (ex is not ValidationException)
        {
            _logger.LogDatabaseError($"{nameof(AddUserHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }
}
