using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Configuration;
using Plitkarka.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Plitkarka.Domain.Services.Authentication;

public class JwtAuthenticationService : IAuthenticationService
{
    public static readonly string IdClaimName = "Id";

    private AuthorizationConfiguration _authorizationConfiguration;

    public JwtAuthenticationService(
        IOptions<AuthorizationConfiguration> authorizationOptions)
    {
        _authorizationConfiguration = authorizationOptions.Value;
    }

    public string Authenticate(User toAuthenticate)
    {
        var claims = new List<Claim>
        {
            new Claim(IdClaimName, toAuthenticate.Id.ToString()),
        };

        // Create hashed credentials for signing payload
        var creds = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha512Signature);

        // Form token using payload and signing credentials
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(_authorizationConfiguration.TokenMinutesLifetime),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    public Guid Authorize(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(
            token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetKey(),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            },
            out var validatedToken);

        var jwtToken = (JwtSecurityToken) validatedToken;

        var userId = Guid.Parse(
            jwtToken.Claims.First(
                claim => claim.Type == IdClaimName).Value);

        return userId;
    }

    private SymmetricSecurityKey GetKey() => new SymmetricSecurityKey(
        System.Text.Encoding.UTF8.GetBytes(_authorizationConfiguration.SecretKey));
}
