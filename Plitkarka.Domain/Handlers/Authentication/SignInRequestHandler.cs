using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Authentication;

public class SignInRequestHandler
    : IRequestHandler<SignInRequest, TokenPair>
{
    private static readonly string ValidationExceptionText = "Wrong email or password";
    private IRepository<UserEntity> _repository { get; init; }
    private ILogger<SignInRequestHandler> _logger { get; init; }
    private IMapper _mapper { get; init; }
    private IEncryptionService _encryptionService { get; init; }
    private IAuthenticationService _authenticationService { get; init; }

    public SignInRequestHandler(
        IRepository<UserEntity> repository,
        ILogger<SignInRequestHandler> logger,
        IMapper mapper,
        IEncryptionService encryptionService,
        IAuthenticationService authenticationService)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _encryptionService = encryptionService;
        _authenticationService = authenticationService; 
    }

    public async Task<TokenPair> Handle(
        SignInRequest request, 
        CancellationToken cancellationToken)
    {
        User? user;

        try
        {
            var userEntity = await _repository.GetAll().FirstOrDefaultAsync(
                user => user.Email == request.Email);

            user = _mapper.Map<User>(userEntity);
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(SignInRequestHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }

        if (user == null)
        {
            throw new ValidationException(ValidationExceptionText);
        }

        var password = _encryptionService.Hash(
            request.Password + user.Salt);

        if(!_encryptionService.CheckEquality(user.Password, password))
        {
            throw new ValidationException(ValidationExceptionText);
        }

        var pair = await _authenticationService.Authenticate(user);

        return pair;
    }
}
