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

namespace Plitkarka.Domain.Middlewares;

public class AuthorizationMiddleware
{
    private RequestDelegate _next { get; init; }
    private IAuthorizationService _authorizationService { get; init; }
    private IMapper _mapper { get; init; }
    private IContextUserService _contextUserService { get; init; }
    private IContextAccessTokenService _contextAccessTokenService { get; init; }

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

    public async Task Invoke(
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

            try
            {
                IRepository<UserEntity> userRepository = context
                    .RequestServices
                    .GetRequiredService<IRepository<UserEntity>>();

                user = await userRepository.GetByIdAsync(userId);

                user.LastLoginDate = DateTime.UtcNow.Date;

                await userRepository.UpdateAsync(user);
            }
            catch
            {
                throw new MySqlException();
            }

            _contextUserService.User = _mapper.Map<User>(user);
        }

        await _next(context);
    }
}
