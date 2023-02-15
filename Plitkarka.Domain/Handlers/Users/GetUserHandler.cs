using MediatR;
using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Microsoft.Extensions.Logging;

namespace Plitkarka.Domain.Handlers.Users;

public class GetUserHandler : IRequestHandler<GetUserQuery, User?>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger _logger { get; init; }

    public GetUserHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILoggerFactory loggerFactory)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        UserEntity? resultEntity;

        try
        {
            resultEntity = await _repository.GetUserAsync(request.Predicate);
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(GetUserHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }

        return resultEntity == null
            ? null
            : _mapper.Map<User>(resultEntity);
    }
}
