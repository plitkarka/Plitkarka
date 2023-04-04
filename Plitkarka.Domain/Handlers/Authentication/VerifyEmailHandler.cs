using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Plitkarka.Domain.Handlers.Users;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.Authorization;
using Plitkarka.Domain.Services.EmailService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Authentication;

public class VerifyEmailHandler 
    : IRequestHandler<VerifyEmailRequest, TokenPair>
{
    private IRepository<UserEntity> _repository { get; init; }
    private ILogger<VerifyEmailHandler> _logger { get; init; }
    private IMapper _mapper { get; init; }
    private IAuthenticationService _authenticationService { get; init; }

    public VerifyEmailHandler(
        IRepository<UserEntity> repository,
        ILogger<VerifyEmailHandler> logger,
        IMapper mapper,
        IAuthenticationService authenticationService)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _authenticationService = authenticationService;
    }

    public async Task<TokenPair> Handle(
        VerifyEmailRequest request,
        CancellationToken cancellationToken)
    {
        UserEntity? userEntity;

        try
        {
            userEntity = await _repository.GetAll().FirstOrDefaultAsync(
                user => user.Email == request.Email);
        }
        catch(Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(AddUserHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }

        if (userEntity == null)
        {
            throw new ValidationException($"No user with email {request.Email} found");
        }

        if (userEntity.EmailCode == EmailService.VerifiedCode)
        {
            throw new ValidationException("User is already verified");
        }

        if (userEntity.EmailCode != request.EmailCode)
        {
            throw new ValidationException("Email code is wrong");
        }

        userEntity.EmailCode = EmailService.VerifiedCode;

        await _repository.UpdateAsync(userEntity);

        var user = _mapper.Map<User>(userEntity);

        var pair = await _authenticationService.Authenticate(user);

        return pair;
    }
   

}
