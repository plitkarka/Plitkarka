using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.ResetPasswordController;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.PasswordManager;
using Plitkarka.Domain.ResponseModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/password/reset")]
public class ResetPasswordController : Controller
{
    private IMediator _mediator { get; init; }

    public ResetPasswordController(
       IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Send an email with code to reset password", 
        Description = $@"
            Check if user with sepcific email exist and send code to reset passwrod on this email.
            Returns email.
            Returns 400 if there is no user with such email
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StringResponse>> SendEmail(
        [FromBody] SendEmailRequestModel body)
    {
        var email = await _mediator.Send(new SendEmailRequest(body.Email));

        return Ok(new StringResponse(email));
    }

    [HttpGet]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Verify code for reseting email",
        Description = $@"
            Check if the code is valid.
            Retuns 400 if there is no user with such email, password reset wasn't requested or code is wrong
        ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyCodeResponse>> VerifyCode(
        [FromQuery] VerifyCodeRequestModel body)
    {
        var email = await _mediator.Send(new VerifyCodeRequest(
            body.Email,
            body.PasswordCode));

        return Ok(email);
    }

    [HttpPut]
    [ModelStateValidation]
    [SwaggerOperation(
        Summary = "Resets password",
        Description = "Sets new password for user. Throw 400 if there is no user with such email, password reset wasn't requested or code is wrong")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenPairResponse>> ResetPassword(
       [FromBody] ResetPasswordRequestModel body)
    {
        var pair = await _mediator.Send(new ResetPasswordRequest(
            body.Email,
            body.PasswordCode,
            body.Password));

        return Ok(pair);
    }
}
