using Microsoft.AspNetCore.Http;

namespace Plitkarka.Domain.Services.ImageService;

public interface IImageService
{
    Task<Guid> UploadImageAsync(IFormFile fileStream);
    string DownloadImage(string keyName);
    Task DeleteImageAsync(Guid imageId);
}

