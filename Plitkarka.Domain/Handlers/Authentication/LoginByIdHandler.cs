using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Authentication;
using Plitkarka.Domain.Services.Authentication;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Authentication;

public class LoginByIdHandler : IRequestHandler<LoginByIdRequest, string>
{
    private IRepository<UserEntity> _userRepository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger<LoginByIdHandler> _logger { get; init; }
    private IAuthenticationService _authenticationService { get; init; }

    public LoginByIdHandler(
        IRepository<UserEntity> userRepository,
        IMapper mapper,
        ILogger<LoginByIdHandler> logger,
        IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public async Task<string> Handle(LoginByIdRequest request, CancellationToken cancellationToken)
    {
        var userEnity = await _userRepository.GetByIdAsync(request.Id);

        var user = _mapper.Map<User>(userEnity);

        return _authenticationService.Authenticate(user);
    }
}
