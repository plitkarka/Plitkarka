using AutoMapper;
using MediatR;
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

public class RefreshTokenPairHandler : IRequestHandler<RefreshTokenPairRequest, TokenPair>
{
    private IAuthenticationService _authenticationService { get; init; }
    private IContextAccessTokenService _contextAccessTokenService { get; init; }
    private IAuthorizationService _authorizationService { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }
    private IMapper _mapper { get; init; }

    public RefreshTokenPairHandler(
        IAuthenticationService authenticationService,
        IContextAccessTokenService contextAccessTokenService,
        IAuthorizationService authorizationService,
        IRepository<UserEntity> userRepository,
        IMapper mapper)
    {
        _authenticationService = authenticationService;
        _contextAccessTokenService = contextAccessTokenService;
        _authorizationService = authorizationService;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<TokenPair> Handle(RefreshTokenPairRequest request, CancellationToken cancellationToken)
    {
        var token = _contextAccessTokenService.AccessToken;

        if (token == null)
        {
            throw new AuthorizationErrorException("Access token not given");
        }

        var userId = _authorizationService.Authorize(token, validateTime: false);

        var userEntity = await _userRepository.GetByIdAsync(userId);

        if (userEntity == null)
        {
            throw new AuthorizationErrorException("User for access token not found");
        }

        var user = _mapper.Map<User>(userEntity);

        var refreshToken = request.RefreshToken;

        if (user.RefreshToken.Token != refreshToken)
        {
            throw new AuthorizationErrorException("Refresh token is wrong");
        }

        if (user.RefreshToken.IsActive != true)
        {
            throw new AuthorizationErrorException("Refresh token is not active");
        }

        if (user.RefreshToken.Expires < DateTime.UtcNow)
        {
            throw new AuthorizationErrorException("Refresh token expired");
        }

        var pair = await _authenticationService.Authenticate(user);

        return pair;
    }
}
