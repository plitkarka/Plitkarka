using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Users;

public class SetUserImageHandler : IRequestHandler<SetUserImageRequest, Guid>
{
    private User _user { get; init; }
    private IImageService _imageService { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }
    private IRepository<UserImageEntity> _userImageRepository { get; init; }

    public SetUserImageHandler(
        IContextUserService contextUserService,
        IImageService imageService,
        IRepository<UserEntity> userRepository,
        IRepository<UserImageEntity> userImageRepository)
    {
        _user = contextUserService.User;
        _imageService = imageService;
        _userRepository = userRepository;
        _userImageRepository = userImageRepository;
    }

    public async Task<Guid> Handle(SetUserImageRequest request, CancellationToken cancellationToken)
    {
        if (request.Image == null)
        {
            throw new ValidationException("Image required");
        }

        UserEntity? user;

        try
        {
            user = await _userRepository
                .GetAll()
                .Include(user => user.UserImage)
                .FirstOrDefaultAsync(user => user.Id == _user.Id);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (user.UserImage != null)
        {
            await _userImageRepository.DeleteAsync(user.UserImage);
        }        

        var imageKey = await _imageService.UploadImageAsync(request.Image);

        var imageEntity = new UserImageEntity()
        {
            User = user,
            ImageKey = imageKey
        };

        var imageId = await _userImageRepository.AddAsync(imageEntity);

        return imageId;
    }
}
