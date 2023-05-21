using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Requests.PasswordManager;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.EmailService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ResetPassword;

public class VerifyCodeHandler : IRequestHandler<VerifyCodeRequest, VerifyCodeResponse>
{
    private IRepository<UserEntity> _repository { get; init; }

    public VerifyCodeHandler(
        IRepository<UserEntity> repository)
    {
        _repository = repository;
    }
    public async Task<VerifyCodeResponse> Handle(VerifyCodeRequest request, CancellationToken cancellationToken)
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

        var response = new VerifyCodeResponse() 
        { 
            Email = userEntity.Email, 
            PasswordCode = userEntity.ChangePasswordCode 
        };

        return response;
    }
}
