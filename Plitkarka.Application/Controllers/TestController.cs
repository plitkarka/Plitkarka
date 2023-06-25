using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : Controller
{
    private IRepository<UserEntity> _userRepository { get; init; }
    private IEncryptionService _encryptionService { get; init; }
    private IMapper _mapper { get; init; }
    private IAuthenticationService _authenticationService { get; init; }

    public TestController(
        IRepository<UserEntity> userRepository,
        IEncryptionService encryptionService,
        IMapper mapper,
        IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _mapper = mapper;
        _authenticationService = authenticationService;
    }

    [HttpPost("defaultUser")]
    [SwaggerOperation(
        Summary = "Creates default user with specified name",
        Description = $@"
            Adds user with prepared credentials to database.
            Default user name is 'admin', but can be set down with request param to create multiple instances
        ")]
    public async Task<ActionResult<TokenPairResponse>> CreateDefaultUser(string name = "admin")
{
        var salt = _encryptionService.GenerateSalt();
        var newUser = new User()
        {
            Login = name,
            Name = name,
            Email = name + "@gmail.com",
            EmailCode = "",
            Password = _encryptionService.Hash("123" + salt),
            Salt = salt,
            BirthDate = DateTime.UtcNow,
            LastLoginDate = DateTime.UtcNow
        };

        newUser.Id = await _userRepository.AddAsync(
            _mapper.Map<UserEntity>(newUser));

        var pair = await _authenticationService.Authenticate(newUser, "adminIdentifier");

        return Ok(pair);
    }


    [HttpPost("defaultUser/many")]
    [SwaggerOperation(
        Summary = "Creates many default users",
        Description = @"
            Adds some count of users with prepared credentials to database.
            Default user name is 'admin', but can be set down with request param to create multiple instances
        ")]
    public async Task<IActionResult> CreateManyDefaultUsers(string name = "admin", int count = 2)
    {
        for (int i = 0; i < count; i++)
        {
            var newName = name + i;

            var salt = _encryptionService.GenerateSalt();

            var newUser = new User()
            {
                Login = newName,
                Name = newName,
                Email = newName + "@gmail.com",
                EmailCode = "",
                Password = _encryptionService.Hash("123" + salt),
                Salt = salt,
                BirthDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow
            };

            await _userRepository.AddAsync(
                _mapper.Map<UserEntity>(newUser));
        }

        return Ok();
    }

    [HttpGet("defaultUser")]
    [SwaggerOperation(
        Summary = "Login with user name",
        Description = $@"
            Login with user name.
            Default user name is 'admin', but can be set down with request
        ")]
    public async Task<ActionResult<TokenPairResponse>> LoginUser(string name = "admin")
    {
        var userEntity = await _userRepository
            .GetAll()
            .FirstOrDefaultAsync(user => user.Name == name);

        var user = _mapper.Map<User>(userEntity);

        var pair = await _authenticationService.Authenticate(user, "adminIdentifier"); 

        return Ok(pair);
    }
}
