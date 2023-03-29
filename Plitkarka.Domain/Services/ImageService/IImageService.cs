using Microsoft.AspNetCore.Http;

namespace Plitkarka.Domain.Services.ImageService;

public interface IImageService
{
    Task<Guid> UploadImageAsync(IFormFile fileStream, string contentType);
    string DownloadImage(string keyName);
    Task DeleteImageAsync(string keyName);
}

