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
    [SwaggerOperation(
        Summary = "User Sign up",
        Description = $@"
            Register new user entity using data sent with request.
            Returns email.
            Returns 400 if Login or Email is already used.
            In this case has response header 'FailedParam' with the name of failed parameter (Email/Login)
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StringResponse>> SignUp(
        [FromBody] SignUpRequestModel body)
    {
        var email = await _mediator.Send(new SignUpRequest(
            Login: body.Login,
            Name: body.Name,
            Email: body.Email,
            Password: body.Password,
            BirthDate: body.BirthDate));

        return Ok(new StringResponse(email));
    }

    [HttpPost("email")]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Verify email", 
        Description = $@"
            Receives email code sent after registration and verifies it.
            Returns 400 if email is not found, email code is wrong or user is already verified.
            If code is wrong has response header 'FailedParam' with value 'EmailCode'
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenPairResponse>> VerifyEmail(
        [FromBody] VerifyEmailRequestModel body)
    {
        var pair = await _mediator.Send(new VerifyEmailRequest(
            body.Email,
            body.EmailCode,
            body.UniqueIdentifier));

        return Ok(pair);
    }

    [HttpPut("email")]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Resend Verification Code", 
        Description = $@"
            Send verification code once more time at certain email.
            Returns email.
            Returns 400 if email is not found or user already verified")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StringResponse>> ResendVerificationCode(
        [FromBody] ResendVerificationCodeRequestModel body)
    {
        var email = await _mediator.Send(new ResendVerificationCodeRequest(body.Email));

        return Ok(new StringResponse(email));
    }

    [HttpPost("signin")]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "User Sign In", 
        Description = $@"
            Login user into his account.
            Return token pair for account access.
            Returns 400 if email or passwrod is wrong
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenPairResponse>> SignIn(
        [FromBody] SignInBodyRequestModel body)
    {
        var pair = await _mediator.Send(new SignInRequest(
            body.Email,
            body.Password,
            body.UniqueIdentifier));

        return Ok(pair);
    }

    [NeedAuthorizeToken]
    [HttpGet("refresh")]
    [SwaggerOperation(
        Summary = "Refresh Token Pair", 
        Description = $@"
            Receive refresh token to return new token pair and unique identifier of device.
            Throws 403 if user have never been authorized with this identifier, access token user not found, refresh token is invalid, not active or expired
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TokenPairResponse>> RefreshTokenPair(
        [FromQuery] RefreshTokenPairRequestModel body)
    {
        var pair = await _mediator.Send(new RefreshTokenPairRequest(
            body.RefreshToken,
            body.UniqueIdentifier));

        return Ok(pair);
    }
}
