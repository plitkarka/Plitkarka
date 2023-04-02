﻿using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Features;
using Plitkarka.Commons.Logger;
using Plitkarka.Domain.Handlers.Users;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.EmailService;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Authentication;

public class SignUpHandler : IRequestHandler<SignUpRequest, string>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger<SignUpHandler> _logger { get; init; }
    private IEmailService _emailService { get; init; }
    private IEncryptionService _encryptionService { get; init; }

    public SignUpHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILogger<SignUpHandler> logger,
        IEmailService emailService,
        IEncryptionService encryptionService,
        IAuthenticationService authenticationService)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _emailService = emailService;
        _encryptionService = encryptionService;
    }

    public async Task<string> Handle(
        SignUpRequest request,
        CancellationToken cancellationToken)
    {
        await ValidateEmailAndlogin(request);

        var salt = _encryptionService.GenerateSalt();

        var newUser = new User()
        {
            Login = request.Login,
            Name = request.Name,
            Email = request.Email,
            EmailCode = _emailService.GenerateEmailCode(),
            Password = _encryptionService.Hash(request.Password + salt),
            Salt = salt,
            BirthDate = request.BirthDate,
            CreatedDate = DateTime.UtcNow,
            LastLoginDate = DateTime.UtcNow,
            IsActive = true
        };

        newUser.Id = await _repository.AddAsync(
            _mapper.Map<UserEntity>(newUser));

        await _emailService.SendEmailAsync(
            newUser.Email,
            EmailTextTemplates.VerificationCodeText(newUser.Name, newUser.EmailCode),
            EmailTextTemplates.VerificationCode);

        return newUser.Email;
    }

    private async Task ValidateEmailAndlogin(SignUpRequest request)
    {
        UserEntity? userExist;

        try
        {
            userExist = await _repository.GetAll().FirstOrDefaultAsync(
               user => user.Email == request.Email || user.Login == request.Login);

            if (userExist != null)
            {
                if (userExist.Email == request.Email)
                {
                    throw new ValidationException("This Email is already used", nameof(request.Email));
                }

                if (userExist.Login == request.Login)
                {
                    throw new ValidationException("This Login is already used", nameof(request.Login));
                }
            }
        }
        catch (Exception ex) when (ex is not ValidationException)
{
            _logger.LogDatabaseError($"{nameof(AddUserHandler)}.{nameof(ValidateEmailAndlogin)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
    }


}
