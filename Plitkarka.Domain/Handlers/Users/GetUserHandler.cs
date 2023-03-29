using MediatR;
using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Microsoft.Extensions.Logging;
using Plitkarka.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Plitkarka.Domain.Handlers.Users;

public class GetUserHandler : IRequestHandler<GetUserRequest, User?>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger<GetUserHandler> _logger { get; init; }

    public GetUserHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILogger<GetUserHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<User?> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        UserEntity? resultEntity;

        try
        {
            resultEntity = await _repository.GetAll().FirstOrDefaultAsync(request.Predicate);
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
