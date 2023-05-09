using Microsoft.AspNetCore.Http;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Exceptions;
using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Services.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ContextAccessToken;
using Plitkarka.Domain.Services.EmailService;

namespace Plitkarka.Domain.Middlewares;

public class AuthorizationMiddleware
{
    protected RequestDelegate _next { get; init; }
    protected IAuthorizationService _authorizationService { get; init; }
    protected IMapper _mapper { get; init; }
    protected IContextUserService _contextUserService { get; init; }
    protected IContextAccessTokenService _contextAccessTokenService { get; init; }

    public AuthorizationMiddleware(
        RequestDelegate next,
        IAuthorizationService authorizationService,
        IMapper mapper,
        IContextUserService contextUserService,
        IContextAccessTokenService contextAccessTokenService)
    {
        _next = next;
        _authorizationService = authorizationService;
        _mapper = mapper;
        _contextUserService = contextUserService;
        _contextAccessTokenService = contextAccessTokenService;
    }

    virtual public async Task InvokeAsync(
        HttpContext context)
    {
        var token = _contextAccessTokenService.AccessToken;

        if (token != null)
        {
            Guid userId;
            UserEntity? user;

            userId = _authorizationService.Authorize(token);

            if (userId == Guid.Empty)
            {
                await _next(context);
                return;
            }

            var userRepository = context
                .RequestServices
                .GetRequiredService<IRepository<UserEntity>>();

            user = await userRepository.GetByIdAsync(userId);

            if (user.EmailCode != EmailService.VerifiedCode)
            {
                throw new AuthorizationErrorException("Email is not verified");
            }

            user.LastLoginDate = DateTime.UtcNow.Date;

            await userRepository.UpdateAsync(user);

            _contextUserService.User = _mapper.Map<User>(user);
        }

        await _next(context);
    }
}
