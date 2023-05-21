using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.Services.EmailService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Authentication;

public class ResendVerificationCodeHandler
    : IRequestHandler<ResendVerificationCodeRequest, string>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IEmailService _emailService { get; init; }

    public ResendVerificationCodeHandler(
        IRepository<UserEntity> repository,
        IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task<string> Handle(ResendVerificationCodeRequest request, CancellationToken cancellationToken)
{
        UserEntity? userEntity;

        try
        {
            userEntity = await _repository.GetAll().FirstOrDefaultAsync(
                user =>  user.IsActive == true && user.Email == request.Email);
        }
        catch (Exception ex)
        {
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

        userEntity.EmailCode = _emailService.GenerateEmailCode();

        await _repository.UpdateAsync(userEntity);

        await _emailService.SendEmailAsync(
            userEntity.Email,
            EmailTextTemplates.VerificationCodeText(userEntity.Name, userEntity.EmailCode),
            EmailTextTemplates.VerificationCode);

        return userEntity.Email;
    }
}
