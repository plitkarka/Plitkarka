using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Users;

public class GetUserDataHandler : IRequestHandler<GetUserDataRequest, UserDataResponse>
{
    private User _user { get; init; }

    private IRepository<UserEntity> _userRepository { get; init; }

    public GetUserDataHandler(
        IContextUserService contextUserService,
        IRepository<UserEntity> userRepository)
    {
        _user = contextUserService.User;
        _userRepository = userRepository;
    }

    public async Task<UserDataResponse> Handle(GetUserDataRequest request, CancellationToken cancellationToken)
    {
        var id = request.UserId == Guid.Empty
            ? _user.Id
            : request.UserId;

        UserDataResponse? response;

        try
        {
            response = await _userRepository.GetAll()
                .Include(u => u.Subscribers)
                .Include(u => u.Subscriptions)
                .Where(u => u.Id == id)
                .Select(user => new UserDataResponse
                {
                    Id = user.Id,
                    Login = user.Login,
                    Name = user.Name,
                    Email = user.Email,
                    BirthDate = user.BirthDate,
                    LastLoginDate = user.LastLoginDate,
                    SubscribersCount = user.Subscribers.Count(),
                    SubscriptionsCount = user.Subscriptions.Count()
                })
                .FirstOrDefaultAsync();
        }
        catch(Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (response == null)
        {
            throw new ValidationException("User not found");
        }

        return response;
    }
}                