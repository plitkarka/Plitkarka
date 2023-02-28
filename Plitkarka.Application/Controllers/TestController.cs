using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Amazon;
using MediatR;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Amazon.S3;
using Plitkarka.Infrastructure.Services.ImageService.Service;
using System.Net.Mime;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("plitkarka/api/test")]
public class TestController : Controller
{
    private IRepository<UserEntity> _userRepository { get; init; }
    private IMapper _mapper { get; init; }
    private IMediator _mediator { get; init; }
    private IImageService _imageService { get; init; }

    public TestController(
        IRepository<UserEntity> userRepository,
        IMapper mapper,
        IMediator mediator,
        IImageService imageService)
    {
        _userRepository = userRepository;  
        _mapper = mapper;
        _mediator = mediator;
        _imageService = imageService;  
    }
    private string accessKey = "AKIARFW4WD5WWZ64CAER";
    private string secretKey = "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";
    [HttpPost]
    public async Task<ActionResult> TestPost(string login)
    {
        var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
        var client = new AmazonS3Client(awsCredentials,RegionEndpoint.EUCentral1);
        //var res = await _imageService.UploadAnImageAsync("C:\\Users\\Daniel\\Desktop\\15.02.22.jpg", "image/jpeg", client);
        //var res = await _imageService.DeleteAnObjectAsync("Hello.txt", client);
        var res =  _imageService.DownloadAnImageAsync("15.02.22.jpg", client);

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
