using Microsoft.AspNetCore.Http;
namespace Plitkarka.Infrastructure.Services.ImageService;

public interface IImageService
{
    Task<Guid> UploadImageAsync(IFormFile fileStream, string contentType);
    string DownloadImage(string keyName);
    Task DeleteImageAsync(string keyName);

}

