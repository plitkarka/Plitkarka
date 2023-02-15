using MediatR;
using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Logger;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Domain.Handlers.Users;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User?>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger _logger { get; init; }
    
    public GetUserByIdHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILoggerFactory loggerFactory)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetUserByIdHandler>();
    }

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        UserEntity? resultEntity;

        try
        {
            resultEntity = await _repository.GetUserByIdAsync(request.Id);
        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(GetUserByIdHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }

        return resultEntity == null 
            ? null
            : _mapper.Map<User>(resultEntity);
    }
}
