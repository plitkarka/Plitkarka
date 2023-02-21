using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Services;

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
    public async Task<ActionResult> TestPost(string login)
    {
        var template = new User()
        {
            FirstName = "SomeName",
            SecondName = "SameSecondName",
            Password = "qwerty",
            BirthDate = DateTime.Now
        };

        var id = await _mediator.Send(new AddUserRequest(
            template with { Login = login, Email = $"{login}@gmail.com" }));

        return Ok(id);
    }

    [HttpGet("id")]
    public async Task<IActionResult> TestGet(Guid id)
    {
        var res = await _mediator.Send(new GetUserByIdQuery(id));

        return Ok(res);
    }


    [HttpGet("login")]
    public async Task<IActionResult> TestGet(string login)
    {
        var res = await _mediator.Send(new GetUserQuery(user => user.Login == login));

        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> TestDelete(Guid id)
    {
        await _mediator.Send(new DeleteUserByIdRequest(id));

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> TestUpdate(Guid id, string login)
    {
        var res = await _mediator.Send(new UpdateUserRequest(new UserUpdate() { Id = id, Login = login}));

        return Ok(res);
    }
}
