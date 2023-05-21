using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PasswordManager;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.EmailService;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ResetPassword;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, TokenPairResponse>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private IAuthenticationService _authenticationService { get; init; }
    private IEncryptionService _encryptionService { get; init; }

    public ResetPasswordHandler(
        IRepository<UserEntity> repository,
        IAuthenticationService authenticationService,
        IEncryptionService encryptionService,
        IMapper mapper)
    {
        _repository = repository;
        _authenticationService = authenticationService;
        _encryptionService = encryptionService;
        _mapper = mapper;
    }
    public async Task<TokenPairResponse> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        UserEntity? userEntity;

        try
        {
            userEntity = await _repository.GetAll().FirstOrDefaultAsync(
                user => user.Email == request.Email);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (userEntity == null)
        {
            throw new ValidationException($"No user with email {request.Email} found");
        }

        if (userEntity.ChangePasswordCode == EmailService.VerifiedCode)
        {
            throw new ValidationException("Password reset wasn't requested");
        }

        if (userEntity.ChangePasswordCode != request.PasswordCode)
        {
            throw new ValidationException("Reset password code is wrong");
        }

        userEntity.ChangePasswordCode = EmailService.VerifiedCode;

        var salt = _encryptionService.GenerateSalt();

        userEntity.Password = _encryptionService.Hash(request.Password + salt);

        userEntity.Salt = salt;

        await _repository.UpdateAsync(userEntity);

        var user = _mapper.Map<User>(userEntity);

        var pair = await _authenticationService.Authenticate(user);

        return pair;
    }
}
