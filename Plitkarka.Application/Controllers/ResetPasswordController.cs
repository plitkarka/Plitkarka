﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Application.Models.ResetPasswordController;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.PasswordManager;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/password")]
public class ResetPasswordController : Controller
{
    private IMediator _mediator { get; init; }

    public ResetPasswordController(
       IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("email")]
    [ModelStateValidation]
    public async Task<ActionResult<string>> SendEmail(
        [FromBody] SendEmailRequestModel body)
    {
        var email = await _mediator.Send(new SendEmailRequest(
            body.Email));

        return Ok(email);
    }

    [HttpGet("code")]
    [ModelStateValidation]
    public async Task<ActionResult<(string,string)>> VerifyCode(
        [FromQuery] VerifyCodeRequestModel body)
    {
        var email = await _mediator.Send(new VerifyCodeRequest(
            body.Email,
            body.PasswordCode));

        return Ok(email);
    }

    [HttpPut("reset")]
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
