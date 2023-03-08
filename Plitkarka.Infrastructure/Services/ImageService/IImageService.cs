using Microsoft.AspNetCore.Http;
namespace Plitkarka.Infrastructure.Services.ImageService;

public interface IImageService
{
    public Task<Guid> UploadImageAsync(IFormFile fileStream, string contentType);
    public string DownloadImage(string keyName);
    public Task DeleteImageAsync(string keyName);

}

