using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Users;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileRequest, UserDataResponse>
{
    private User _user { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }
    private IImageService _imageService { get; init; }

    public UpdateProfileHandler(
        IContextUserService contextUserService,
        IRepository<UserEntity> userRepository,
        IImageService imageService)
    {
        _user = contextUserService.User;
        _userRepository = userRepository;
        _imageService = imageService;
    }

    public async Task<UserDataResponse> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    { 
        if (request.Login.IsNullOrEmpty() &&
            request.Name.IsNullOrEmpty() &&
            request.Description.IsNullOrEmpty() && 
            request.Link.IsNullOrEmpty())
        {
            throw new ValidationException("Nothing to update");
        }

        UserEntity user;

        try
        {
            user = await _userRepository
                .GetAll()
                .Include(user => user.Subscribers)
                .Include(user => user.Subscriptions)
                .Include(u => u.UserImage)
                .Where(user => user.Id == _user.Id)
                .FirstOrDefaultAsync() ?? throw new UserContextException();
        }
        catch (Exception ex) when (ex is not UserContextException)
        {
            throw new MySqlException(ex.Message);
        }

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

        var result = new UserDataResponse
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Email = user.Email,
            Description = user.Description,
            Link = user.Link,
            BirthDate = user.BirthDate,
            LastLoginDate = user.LastLoginDate,
            SubscribersCount = user.Subscribers.Count(),
            SubscriptionsCount = user.Subscriptions.Count(),
            ImageUrl = null,
            IsAuthorized = true,
            IsSubscribed = false
        };

        if (user.UserImage != null)
        {
            result.ImageUrl = _imageService.GetImageUrl(user.UserImage.ImageKey);
        }

        return result;
    }
}
