using AutoMapper;
using MediatR;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class ConnectHandler : IRequestHandler<ConnectRequest>
{
    private User _user { get; init; }
    private IRepository<HubConnectionEntity> _connectionRepository { get; init; }
    private IMapper _mapper { get; init; }

    public ConnectHandler(
        IContextUserService contextUserService,
        IRepository<HubConnectionEntity> connectionRepository,
        IMapper mapper)
    {
        _user = contextUserService.User;
        _connectionRepository = connectionRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(ConnectRequest request, CancellationToken cancellationToken)
    {
        var connection = new HubConnection
        {
            ConnectionId = request.ConnectionId,
            UserId = _user.Id
        };

        var connectionEntity = _mapper.Map<HubConnectionEntity>(connection);

        await _connectionRepository.AddAsync(connectionEntity);

        return Unit.Value;
    }
}
