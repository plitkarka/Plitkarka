using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Configuration;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Services.Authentication;

namespace Plitkarka.Domain.Services.Authorization;

public class JwtAuthorizationService : IAuthorizationService
{
    private AuthorizationConfiguration _authorizationConfiguration { get; init; }

    public JwtAuthorizationService(
        IOptions<AuthorizationConfiguration> authorizationOptions)
    {
        _authorizationConfiguration = authorizationOptions.Value;
    }

    /// <summary>
    /// Authorize user from token data
    /// </summary>
    /// <param name="token">User access token</param>
    /// <returns>Id of authorized user. Returns Guid.Empty if token is expired </returns>
    /// <exception cref="InvalidTokenException">If token is invalid</exception>
    public Guid Authorize(string token, bool validateTime = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken? validatedToken;

        try
        {
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = GetKey(),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false,
                },
                out validatedToken);
        }
        catch
        {
            throw new InvalidTokenException("Token is invalid");
        }
        
        var jwtToken = (JwtSecurityToken) validatedToken;
        
        if (validateTime && jwtToken.ValidTo < DateTime.UtcNow)
        {
            return Guid.Empty;
        }

        var userId = Guid.Parse(
            jwtToken.Claims.First(
                claim => claim.Type == JwtAuthenticationService.IdClaimName).Value);

        return userId;
    }

    private SymmetricSecurityKey GetKey() => new SymmetricSecurityKey(
        System.Text.Encoding.UTF8.GetBytes(_authorizationConfiguration.SecretKey));
}
