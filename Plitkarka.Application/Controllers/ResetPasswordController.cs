using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.ResetPasswordController;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PasswordManager;
using Plitkarka.Domain.ResponseModels;

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
    public async Task<ActionResult<string>> SendEmail(
        [FromBody] SendEmailRequestModel body)
    {
        var email = await _mediator.Send(new SendEmailRequest(
            body.Email));

        return Ok(email);
    }

    [HttpGet]
    [ModelStateValidation]
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
    public async Task<ActionResult<TokenPair>> ResetPassword(
       [FromBody] ResetPasswordRequestModel body)
    {
        var pair = await _mediator.Send(new ResetPasswordRequest(
            body.Email,
            body.PasswordCode,
            body.Password));

        return Ok(pair);
    }
}
