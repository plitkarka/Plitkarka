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
    public static readonly string JwtCookiesKey = "jwtToken";
    public static readonly int JwtTokenExpirationTime = 1;

    private AuthorizationConfiguration _authorizationOptions;

    public JwtAuthenticationService(
        IOptions<AuthorizationConfiguration> authorizationOptions)
    {
        _authorizationOptions = authorizationOptions.Value;
    }

    public string Authenticate(User toAuthenticate)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, toAuthenticate.Login),
        };

        // Build secret key 
        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_authorizationOptions.SecretKey));

        // Create hashed credentials for signing payload
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Form token using payload and signing credentials
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(_authorizationOptions.TokenMinutesLifetime),
            signingCredentials: creds);

        // Build token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void LogOut()
    {
        
    }
}
