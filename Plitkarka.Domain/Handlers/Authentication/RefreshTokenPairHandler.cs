using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Domain.Services.Authorization;
using Plitkarka.Domain.Services.ContextAccessToken;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Authentication;

public class RefreshTokenPairHandler : IRequestHandler<RefreshTokenPairRequest, TokenPairResponse>
{
    private IAuthenticationService _authenticationService { get; init; }
    private IContextAccessTokenService _contextAccessTokenService { get; init; }
    private IAuthorizationService _authorizationService { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }
    private IRepository<RefreshTokenEntity> _tokenRepository { get; init; }
    private IMapper _mapper { get; init; }

    public RefreshTokenPairHandler(
        IAuthenticationService authenticationService,
        IContextAccessTokenService contextAccessTokenService,
        IAuthorizationService authorizationService,
        IRepository<UserEntity> userRepository,
        IRepository<RefreshTokenEntity> tokenRepository,
        IMapper mapper)
    {
        _authenticationService = authenticationService;
        _contextAccessTokenService = contextAccessTokenService;
        _authorizationService = authorizationService;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }

    public async Task<TokenPairResponse> Handle(RefreshTokenPairRequest request, CancellationToken cancellationToken)
    {
        var token = _contextAccessTokenService.AccessToken;

        if (token == null)
        {
            throw new AuthorizationErrorException("Access token not given");
        }

        var userId = _authorizationService.Authorize(token, validateTime: false);

        UserEntity? userEntity;
        RefreshTokenEntity? refreshToken;

        try
        {
            userEntity = await _userRepository
                .GetAll()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userEntity == null)
            {
                throw new AuthorizationErrorException("User for access token not found");
            }

            refreshToken = await _tokenRepository
                .GetAll()
                .FirstOrDefaultAsync(token => token.UserId == userId && token.UniqueIdentifier == request.UniqueIdentifier);
        }
        catch (Exception ex) when (ex is not AuthorizationErrorException)
        {
            throw new MySqlException(ex.Message);
        }

        if (refreshToken == null)
        {
            throw new AuthorizationErrorException("User have never been authorized with this identifier");
        }

        if (refreshToken.Token != request.RefreshToken)
        {
            throw new AuthorizationErrorException("Refresh token is wrong. Also it can be token for other device");
        }

        if (refreshToken.Expires < DateTime.UtcNow)
        {
            throw new AuthorizationErrorException("Refresh token expired");
        }

        var pair = await _authenticationService.Authenticate(
            _mapper.Map<User>(userEntity), request.UniqueIdentifier);

        return pair;
    }
}
