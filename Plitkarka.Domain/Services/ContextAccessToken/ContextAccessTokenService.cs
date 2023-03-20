using Microsoft.AspNetCore.Http;

namespace Plitkarka.Domain.Services.ContextAccessToken;

public class ContextAccessTokenService : IContextAccessTokenService
{
    public static readonly string AuthorizationHeaderName = "AuthToken";

    private IHttpContextAccessor _httpContextAccessor { get; init; }

    public ContextAccessTokenService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? AccessToken
    {
        get => _httpContextAccessor.HttpContext.Request.Headers[AuthorizationHeaderName]
            .FirstOrDefault()
            ?.Split(" ")
            .Last();
    }
}
