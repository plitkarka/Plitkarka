using Microsoft.AspNetCore.Http;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Exceptions;
using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Services.Authorization;

namespace Plitkarka.Domain.Middlewares;

public class AuthorizationMiddleware
{
    public static readonly string AuthorizationHeaderName = "AuthToken";
    private RequestDelegate _next { get; init; }
    private IAuthorizationService _authorizationService { get; init; }
    private IMapper _mapper { get; init; }

    public AuthorizationMiddleware(
        RequestDelegate next,
        IAuthorizationService authorizationService,
        IMapper mapper)
    {
        _next = next;
        _authorizationService = authorizationService;
        _mapper = mapper;
    }

    public async Task Invoke(
        HttpContext context,
        IRepository<UserEntity> userRepository)
    {
        var token = context.Request.Headers[AuthorizationHeaderName]
            .FirstOrDefault()
            ?.Split(" ")
            .Last();

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
                user = await userRepository.GetByIdAsync(userId);
                user.LastLoginDate = DateTime.UtcNow.Date;
                await userRepository.UpdateAsync(user);
            }
            catch
            {
                throw new MySqlException();
            }
          
            context.Items["User"] = _mapper.Map<User>(user);
        }

        await _next(context);
    }
}
