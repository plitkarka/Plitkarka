using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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

public class SubscribeHandler : IRequestHandler<SubscribeRequest, Guid>
{
    private User _user { get; init; }
    private IRepository<SubscriptionEntity> _subscriptionRepository { get; init; }
    private IRepository<UserEntity> _userRepository { get; set; }
    private IMapper _mapper { get; init; }

    public SubscribeHandler(
        IContextUserService contextUserService,
        IRepository<SubscriptionEntity> subscriptionRepository,
        IRepository<UserEntity> userRepository,
        IMapper mapper)
    {
        _user = contextUserService.User;
        _subscriptionRepository = subscriptionRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(SubscribeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.SusbcribeToId);

        if (user == null)
        {
            throw new ValidationException("No user found");
        }

        SubscriptionEntity? existing;

        try
        {
            existing = await _subscriptionRepository.GetAll().FirstOrDefaultAsync(
                pl => pl.SubscribedToId == request.SusbcribeToId && pl.UserId == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (existing != null)
        {
            if (existing.IsActive)
            {
                throw new ValidationException("User already subscribed");
            }
            else
            {
                existing.IsActive = true;

                await _subscriptionRepository.UpdateAsync(existing);

                return existing.Id;
            }
        }

        var newLike = new Subscription()
        {
            UserId = _user.Id,
            SubscribedToId = request.SusbcribeToId,
        };

        var likeEntity = _mapper.Map<SubscriptionEntity>(newLike);

        var id = await _subscriptionRepository.AddAsync(likeEntity);

        return id;
    }
}
