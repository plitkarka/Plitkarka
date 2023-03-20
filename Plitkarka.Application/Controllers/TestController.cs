using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using Plitkarka.Commons.Features;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.Filters;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Infrastructure.Services.ImageService;
using Plitkarka.Infrastructure.Services.EmailService;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : Controller
{
    private IRepository<UserEntity> _userRepository { get; init; }
    private IMapper _mapper { get; init; }
    private IMediator _mediator { get; init; }
    private IImageService _imageService { get; init; }
    private IEmailService _emailService { get; init; }

    public TestController(
        IRepository<UserEntity> userRepository,
        IMapper mapper,
        IMediator mediator,
        IImageService imageService,
        IEmailService emailService)
    {
        _userRepository = userRepository;  
        _mapper = mapper;
        _mediator = mediator;
        _imageService = imageService;  
        _emailService = emailService;
    }
    [HttpPost("sendmail")]
    public async Task<IActionResult> TestPost(string name)
    {

        //var res = await _imageService.UploadImageAsync(file, file.ContentType);
        await _emailService.SendEmailAsync("javaseniorweb@gmail.com", EmailTextTemplates.VerificationCodeText(name, "334546"), EmailTextTemplates.VerificationCode);
        /*var template = new User()
        {
            FirstName = "SomeName",
            SecondName = "SameSecondName",
            Password = "qwerty",
            PasswordAttempts = 3,
            Salt = "12345678",
            BirthDate = DateTime.Now,
            CreatedDate = DateTime.Now,
            LastLoginDate = DateTime.Now,
            IsActive = true
        };

        var id = await _mediator.Send(new AddUserRequest(
            template with { Login = login, Email = $"{login}@gmail.com" }));

        var token = await _mediator.Send(new LoginByIdRequest(id));

        return Json(new { Id = id, Token = token});*/

        return Ok();
    }

    [HttpGet("id")]
    public async Task<IActionResult> TestGet(Guid id)
    {
        var res = await _mediator.Send(new GetUserByIdRequest(id));

        return Ok(res);
    }


    [HttpGet("login")]
    public async Task<IActionResult> TestGet(string login)
    {
        var res = await _mediator.Send(new GetUserRequest(user => user.Login == login));

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

    [Authorize]
    [HttpGet]
    public IActionResult CheckAuthorization()
    {
        var user = HttpContext.Items["User"];

        return Ok(user);
    }
}
