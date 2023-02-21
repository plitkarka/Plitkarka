using MediatR;
using AutoMapper;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;
using Microsoft.Extensions.Logging;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Users;

public class DeleteUserByIdHandler : IRequestHandler<DeleteUserByIdRequest>
{
    private IRepository<UserEntity> _repository { get; init; }
    private ILogger<DeleteUserByIdHandler> _logger { get; init; }

    public DeleteUserByIdHandler(
        IRepository<UserEntity> repository,
        ILogger<DeleteUserByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
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
        catch(Exception ex) when (ex is not ValidationException)
        {
            _logger.LogDatabaseError($"{nameof(DeleteUserByIdHandler)}.{nameof(Handle)}", ex.Message);
            throw new MySqlException(ex.Message);
        }
        
    }
}
