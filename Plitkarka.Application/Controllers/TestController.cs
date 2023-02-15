using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("plitkarka/api/test")]
public class TestController : Controller
{
    private IRepository<UserEntity> _userRepository { get; init; }
    private IMapper _mapper { get; init; }
    private IMediator _mediator { get; init; }

    public TestController(
        IRepository<UserEntity> userRepository,
        IMapper mapper,
        IMediator mediator)
    {
        _userRepository = userRepository;  
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> TestPost()
    {
        var template = new User()
        {
            FirstName = "SomeName",
            SecondName = "SameSecondName",
            EmailCode = "123456",
            Password = "qwerty",
            PasswordAttempts = 3,
            Salt = "12345678",
            BirthDate = DateTime.Now,
            CreatedDate = DateTime.Now,
            LastLoginDate = DateTime.Now,
            IsActive = true
        };

        // await _userRepository.AddUserAsync(_mapper.Map<UserEntity>(
        //     template with { Email = "dima@gmail.com", Login = "Dima" }));

        // await _userRepository.AddUserAsync(_mapper.Map<UserEntity>(
        //     template with { Email = "nastia@gmail.com", Login = "Nastia" }));

        // await _userRepository.AddUserAsync(_mapper.Map<UserEntity>(
        //     template with { Email = "dania@gmail.com", Login = "Dania" }));

        // var id = await _userRepository.AddUserAsync(_mapper.Map<UserEntity>(
        //     template with { Email = "kate@gmail.com", Login = "Kate" }));

        var id = await _userRepository.AddUserAsync(_mapper.Map<UserEntity>(
            template with { Email = "sasha@gmail.com", Login = "Sasha" }));

        return Ok(id);
    }

    [HttpGet]
    public async Task<IActionResult> TestGet(Guid id)
    {
        var res = await _mediator.Send(new GetUserByIdQuery(id));

        return Ok(res);
    }
}
