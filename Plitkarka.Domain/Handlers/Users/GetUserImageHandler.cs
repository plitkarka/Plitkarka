using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.Users;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.Users;

public class GetUserImageHandler : IRequestHandler<GetUserImageRequest, string>
{
    private User _user { get; init; }
    private IImageService _imageService { get; init; }
    private IRepository<UserEntity> _userRepository { get; init; }

    public GetUserImageHandler(
        IContextUserService contextUserService,
        IImageService imageService,
        IRepository<UserEntity> userRepository)
{
        _user = contextUserService.User;
        _imageService = imageService;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(GetUserImageRequest request, CancellationToken cancellationToken)
    {
        var user = request.UserId == Guid.Empty
            ? await GetUser(_user.Id)
            : await GetUser(request.UserId);

        if (user == null)
        {
            throw new ValidationException("User not found");
        }

        if (user.UserImage == null)
        {
            return String.Empty;
        }

        var url = _imageService.GetImageUrl(user.UserImage.ImageKey);

        return url;
    }

    private async Task<UserEntity?> GetUser(Guid id)
    {
        var res = await TryGetAllEntities.Cover(async () =>
        {
            var user = await _userRepository
               .GetAll()
               .Include(user => user.UserImage)
               .FirstOrDefaultAsync(user => user.Id == id);

            return user;
        });

        return res;
    }
}
