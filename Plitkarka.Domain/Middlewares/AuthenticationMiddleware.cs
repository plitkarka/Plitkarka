using Microsoft.AspNetCore.Http;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Exceptions;
using AutoMapper;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Middlewares;

public class AuthenticationMiddleware
{
    public static readonly string AuthorizationHeaderName = "AuthToken";
    private RequestDelegate _next { get; init; }
    private IAuthenticationService _authenticationService { get; init; }
    private IMapper _mapper { get; init; }

    public AuthenticationMiddleware(
        RequestDelegate next,
        IAuthenticationService authenticationService,
        IMapper mapper)
    {
        _next = next;
        _authenticationService = authenticationService;
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

            try
            {
                userId = _authenticationService.Authorize(token);
            }
            catch
            {
                throw new InvalidTokenException();
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
