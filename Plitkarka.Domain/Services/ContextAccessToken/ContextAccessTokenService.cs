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
        get
        {
            var token = GetHeaderValue(AuthorizationHeaderName);

            if (token == null)
            {
                token = GetHeaderValue("Authorization");
            }

            return token;
        }
    }

    private string? GetHeaderValue(string headerName)
    {
        return _httpContextAccessor.HttpContext.Request.Headers[headerName]
            .FirstOrDefault()
            ?.Split(" ")
            .Last();
    }
}
