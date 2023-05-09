using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.AuthController;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "User Sign up", Description = "Register new user entity using specified data sent with request")]
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
    [SwaggerOperation(Summary = "Verify email", Description = "Receives email code sent after registration and verifies it")]
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
    [SwaggerOperation(Summary = "Resend Verification Code", Description = "Send verification code once more time at certain email")]
    public async Task<ActionResult<string>> ResendVerificationCode(
        [FromBody] ResendVerificationCodeRequestModel body)
    {
        var email = await _mediator.Send(new ResendVerificationCodeRequest(
            body.Email));

        return Ok(email);
    }

    [HttpPost("signin")]
    [ModelStateValidation]
    [SwaggerOperation(Summary = "User Sign In", Description = "Login user into his account. Return token pair for account access")]
    public async Task<ActionResult<TokenPair>> SignIn(
        [FromBody] SignInBodyRequestModel body)
    {
        var pair = await _mediator.Send(new SignInRequest(
            Email: body.Email,
            Password: body.Password));

        return Ok(pair);
    }

    [NeedAuthorizeToken]
    [HttpGet("refresh")]
    [SwaggerOperation(Summary = "Refresh Token Pair", Description = "Receive refresh token to return new token pair")]
    public async Task<ActionResult<TokenPair>> RefreshTokenPair(
        [FromQuery] string refreshToken)
    {
        var pair = await _mediator.Send(new RefreshTokenPairRequest(refreshToken));

        return Ok(pair);
    }
}
