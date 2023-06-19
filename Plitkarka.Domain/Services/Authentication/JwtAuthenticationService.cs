using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Configuration;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Plitkarka.Domain.Services.Authentication;

public class JwtAuthenticationService : IAuthenticationService
{
    public static readonly string IdClaimName = "Id";

    private AuthorizationConfiguration _authorizationConfiguration { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }
    private IRepository<RefreshTokenEntity> _refreshTokenRepository { get; init; }
    private IMapper _mapper { get; init; }

    public JwtAuthenticationService(
        IOptions<AuthorizationConfiguration> authorizationOptions,
        IRepository<UserEntity> userRepository,
        IRepository<RefreshTokenEntity> refreshTokenRepository,
        IMapper mapper)
    {
        _authorizationConfiguration = authorizationOptions.Value;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Create a pair of Access and Refresh tokens for specific user. Update database with new token for user
    /// </summary>
    /// <param name="toAuthenticate">User that needs to be authenticated</param>
    /// <returns>Pair of tokens</returns>
    public async Task<TokenPairResponse> Authenticate(User toAuthenticate)
    {
        UserEntity? userEntity;

        try
        {
            userEntity = await _userRepository
                .GetAll()
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.Id == toAuthenticate.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (userEntity == null)
        {
            throw new AuthorizationErrorException("Authorized used not found");
        }

        var claims = new List<Claim>
        {
            new Claim(IdClaimName, toAuthenticate.Id.ToString()),
        };

        // Create hashed credentials for signing payload
        var creds = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha512Signature);

        // Form token using payload and signing credentials
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(_authorizationConfiguration.AccessTokenMinutesLifetime),
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = await GenerateRefreshTokenForUser(userEntity);

        var tokenPair =  new TokenPairResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        return tokenPair;
    }

    private async Task<string> GenerateRefreshTokenForUser(UserEntity userEntity)
    { 
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        if (userEntity.RefreshToken != null)
        {
            var oldRefreshTokenEntity = await _refreshTokenRepository.GetByIdAsync(userEntity.RefreshToken.Id);

            if (oldRefreshTokenEntity != null)
            {
                await _refreshTokenRepository.DeleteAsync(oldRefreshTokenEntity);
            }
        }

        var refreshToken = new RefreshToken()
        {
            Token = token,
            Expires = DateTime.Now.AddMinutes(_authorizationConfiguration.RefreshTokenDaysLifetime),
        };

        var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshToken);
        var newTokenId = await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        userEntity.RefreshTokenId = newTokenId;
        await _userRepository.UpdateAsync(userEntity);

        return token;
    }

    private SymmetricSecurityKey GetKey() => new SymmetricSecurityKey(
        System.Text.Encoding.UTF8.GetBytes(_authorizationConfiguration.SecretKey));
}
