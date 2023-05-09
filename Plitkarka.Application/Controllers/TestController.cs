using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.Encryption;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

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
    public async Task<IActionResult> TestPost(string name = "admin")
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

        var pair = await _authenticationService.Authenticate(newUser);

        return Ok(pair);
    }
}
