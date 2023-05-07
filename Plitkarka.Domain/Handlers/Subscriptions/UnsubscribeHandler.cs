using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Subscriptions;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Repositories;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Subscriptions;

public class UnsubscribeHandler : IRequestHandler<UnsubscribeRequest>
{
    private User _user { get; init; }
    private IRepository<SubscriptionEntity> _subscriptionRepository { get; init; }
    private IRepository<UserEntity> _userRepository { get; set; }

    public UnsubscribeHandler(
        IContextUserService contextUserService,
        IRepository<SubscriptionEntity> subscriptionRepository,
        IRepository<UserEntity> userRepository)
    {
        _user = contextUserService.User;
        _subscriptionRepository = subscriptionRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UnsubscribeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UnsubscribeFromId);

        if (user == null || !user.IsActive)
        {
            throw new ValidationException("No user found");
        }

        SubscriptionEntity? existing;

        try
        {
            existing = await _subscriptionRepository.GetAll().FirstOrDefaultAsync(
                s => s.SubscribedToId == request.UnsubscribeFromId && s.UserId == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing == null || !existing.IsActive)
        {
            throw new ValidationException("No subscription found");
        }

        await _subscriptionRepository.DeleteAsync(existing);

        return Unit.Value;
    }
}
