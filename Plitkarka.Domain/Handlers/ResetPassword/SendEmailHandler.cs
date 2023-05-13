using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Features;
using Plitkarka.Domain.Requests.PasswordManager;
using Plitkarka.Domain.Services.EmailService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ResetPassword;

public class SendEmailHandler : IRequestHandler<SendEmailRequest, string>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IEmailService _emailService { get; init; }

    public SendEmailHandler(
        IRepository<UserEntity> repository,
        IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }
    public async Task<string> Handle(SendEmailRequest request, CancellationToken cancellationToken)
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

        userEntity.ChangePasswordCode = _emailService.GenerateEmailCode();

        await _repository.UpdateAsync(userEntity);

        await _emailService.SendEmailAsync(
            userEntity.Email,
            EmailTextTemplates.ResetPasswordCodeText(userEntity.Name, userEntity.ChangePasswordCode),
            EmailTextTemplates.ResetPasswordCode);

        return userEntity.Email;
    }
}
