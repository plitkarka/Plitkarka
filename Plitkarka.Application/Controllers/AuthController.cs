using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Authentication;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private IMediator _mediator { get; init; }

    public AuthController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ModelStateValidation]
    public async Task<ActionResult<string>> SignUp(
        [FromBody] SignUpRequestModel body)
    {
        var email = await _mediator.Send(new SignUpRequest(
            Login: body.Login,
            Name: body.Name,
            Email: body.Email,
            Password: body.Password,
            BirthDate: body.BirthDate));

        return Ok(email);
    }

    [HttpPost("email")]
    [ModelStateValidation]
    public async Task<ActionResult<TokenPair>> VerifyEmail(
        [FromBody] VerifyEmailRequestModel body)
    {
        var pair = await _mediator.Send(new VerifyEmailRequest(
            body.Email,
            body.EmailCode));

        return Ok(pair);
    }

    [HttpPut("email")]
    [ModelStateValidation]
    public async Task<ActionResult<string>> ResendVerificationCode(
        [FromBody] ResendVerificationCodeRequestModel body)
    {
        var email = await _mediator.Send(new ResendVerificationCodeRequest(
            body.Email));

        return Ok(email);
    }

    [HttpPost("signin")]
    [ModelStateValidation]
    public async Task<ActionResult<TokenPair>> SignIn(
        [FromBody] SignInBodyRequestModel body)
    {
        var pair = await _mediator.Send(new SignInRequest(
            Email: body.Email,
            Password: body.Password));

        return Ok(pair);
    }

    [HttpGet("refresh")]
    public async Task<ActionResult<TokenPair>> RefreshTokenPair(
        [FromQuery] string refreshToken)
    {
        var pair = await _mediator.Send(new RefreshTokenPairRequest(refreshToken));

        return Ok(pair);
    }
}
