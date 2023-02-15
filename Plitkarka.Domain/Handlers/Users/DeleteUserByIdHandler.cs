using MediatR;
using AutoMapper;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Microsoft.Extensions.Logging;

namespace Plitkarka.Domain.Handlers.Users;

public class DeleteUserByIdHandler : IRequestHandler<DeleteUserByIdRequest>
{
    private IRepository<UserEntity> _repository { get; init; }
    private ILogger _logger { get; init; }

    public DeleteUserByIdHandler(
        IRepository<UserEntity> repository,
        ILoggerFactory loggerFactory)
    {
        _repository = repository;
        _logger = loggerFactory.CreateLogger<DeleteUserByIdHandler>();
    }

    public async Task<Unit> Handle(DeleteUserByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var toDelete = await _repository.GetUserByIdAsync(request.Id);

            if (toDelete == null)
            {
                throw new ValidationException("User do not exist");
            }

            await _repository.DeleteUserAsync(toDelete);

            return Unit.Value;
        }
        catch(Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(DeleteUserByIdHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
        
    }
}
