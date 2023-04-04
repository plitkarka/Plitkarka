using MediatR;
using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Logger;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Users;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, User?>
{
    private IRepository<UserEntity> _repository { get; init; }
    private IMapper _mapper { get; init; }
    private ILogger<GetUserByIdHandler> _logger { get; init; }
    
    public GetUserByIdHandler(
        IRepository<UserEntity> repository,
        IMapper mapper,
        ILogger<GetUserByIdHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<User?> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        UserEntity? resultEntity;
        
        resultEntity = await _repository.GetByIdAsync(request.Id);

        return resultEntity == null 
            ? null
            : _mapper.Map<User>(resultEntity);
    }
}
