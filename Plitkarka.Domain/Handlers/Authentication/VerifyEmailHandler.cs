using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.EmailService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Authentication;

public class VerifyEmailHandler 
    : IRequestHandler<VerifyEmailRequest, TokenPairResponse>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private IAuthenticationService _authenticationService { get; init; }

    public VerifyEmailHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        IAuthenticationService authenticationService)
    {
        _repository = repository;
        _mapper = mapper;
        _authenticationService = authenticationService;
    }

    public async Task<TokenPairResponse> Handle(
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
            throw new ValidationException("Email code is wrong", nameof(request.EmailCode));
        }

        userEntity.EmailCode = EmailService.VerifiedCode;

        await _repository.UpdateAsync(userEntity);

        var user = _mapper.Map<User>(userEntity);

        var pair = await _authenticationService.Authenticate(user);

        return pair;
    }
   

}
