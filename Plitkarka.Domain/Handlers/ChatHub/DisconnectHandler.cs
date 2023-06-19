using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Icao;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class DisconnectHandler : IRequestHandler<DisconnectRequest>
{
    private User _user { get; init; }
    private IRepository<HubConnectionEntity> _connectionRepository { get; init; }

    public DisconnectHandler(
        IContextUserService contextUserService,
        IRepository<HubConnectionEntity> connectionRepository)
    {
        _user = contextUserService.User;
        _connectionRepository = connectionRepository;
    }

    public async Task<Unit> Handle(DisconnectRequest request, CancellationToken cancellationToken)
    {
        HubConnectionEntity? connection;

        try
        {
            connection = await _connectionRepository.GetAll().FirstOrDefaultAsync(
                entity => entity.UserId == _user.Id && entity.ConnectionId == request.ConnectionId);
        }
        catch(Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (connection != null)
        {
            await _connectionRepository.DeleteAsync(connection);
        }

        return Unit.Value;
    }
}
