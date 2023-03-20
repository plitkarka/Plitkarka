using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plitkarka.Domain.Requests.Authentication;

namespace Plitkarka.Application.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private IMediator _mediator { get; init; }

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/refresh")]
    public async Task<IActionResult> RefreshTokenPair(string refreshToken)
    {
        var pair = await _mediator.Send(new RefreshTokenPairRequest(refreshToken));

        return Ok(pair);
    }
}
