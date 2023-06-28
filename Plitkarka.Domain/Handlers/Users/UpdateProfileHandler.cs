using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Users;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileRequest>
{
    private User _user { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }

    public UpdateProfileHandler(
        IContextUserService contextUserService,
        IRepository<UserEntity> userRepository)
    {
        _user = contextUserService.User;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    { 
        if (request.Login.IsNullOrEmpty() &&
            request.Name.IsNullOrEmpty() &&
            request.Description.IsNullOrEmpty() && 
            request.Link.IsNullOrEmpty())
        {
            throw new ValidationException("Nothing to update");
        }

        var user = await _userRepository.GetByIdAsync(_user.Id) ?? throw new UserContextException();

        if (!request.Login.IsNullOrEmpty())
        {
            try
            {
                var userExist = await _userRepository
                    .GetAll()
                    .FirstOrDefaultAsync(user => user.Login == request.Login);

                if (userExist != null)
                {
                    throw new ValidationException("This Login is already used", nameof(request.Login));
                }
            }
            catch (Exception ex) when (ex is not ValidationException)
            {
                throw new MySqlException(ex.Message);
            }

            user.Login = request.Login!;
        }

        if (!request.Name.IsNullOrEmpty())
        {
            user.Name = request.Name!;
        }

        if (!request.Description.IsNullOrEmpty())
        {
            user.Description = request.Description!;
        }

        if (!request.Link.IsNullOrEmpty())
        {
            user.Link = request.Link!;
        }

        await _userRepository.UpdateAsync(user);

        return Unit.Value;
    }
}
