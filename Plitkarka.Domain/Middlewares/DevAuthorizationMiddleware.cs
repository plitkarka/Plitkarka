using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Services.Authorization;
using Plitkarka.Domain.Services.ContextAccessToken;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Middlewares;

public class DevAuthorizationMiddleware : AuthorizationMiddleware
{
    private const string AdminUserName = "admin";

    public DevAuthorizationMiddleware(
        RequestDelegate next,
        IAuthorizationService authorizationService,
        IMapper mapper,
        IContextUserService contextUserService,
        IContextAccessTokenService contextAccessTokenService)
    : base (
        next,
        authorizationService,
        mapper, 
        contextUserService,
        contextAccessTokenService)
    {

    }

    override public async Task InvokeAsync(
        HttpContext context)
    {
        var token = _contextAccessTokenService.AccessToken;

        if (token != null)
        {
            await base.InvokeAsync(context);

            return;
        }

        UserEntity? user;

        var userRepository = context
            .RequestServices
            .GetRequiredService<IRepository<UserEntity>>();

        try
        {
            user = await userRepository.GetAll().FirstOrDefaultAsync(
                user => user.Login == AdminUserName);
        }
        catch(Exception ex)
        {
            throw new MySqlException(ex.Message);
}

        if (user == null)
        {
            var encryptionService = context
                .RequestServices
                .GetRequiredService<IEncryptionService>();

            var salt = encryptionService.GenerateSalt();

            var newUser = new User()
            {
                Login = AdminUserName,
                Name = AdminUserName,
                Email = AdminUserName + "@gmail.com",
                EmailCode = "",
                Password = encryptionService.Hash("123" + salt),
                Salt = salt,
                BirthDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow
            };

            newUser.Id = await userRepository.AddAsync(
                _mapper.Map<UserEntity>(newUser));

            user = await userRepository.GetByIdAsync(newUser.Id);
        }

        user.LastLoginDate = DateTime.UtcNow.Date;

        await userRepository.UpdateAsync(user);

        _contextUserService.User = _mapper.Map<User>(user);

        await _next(context);
    }
}
