using Microsoft.AspNetCore.Http;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Domain.Services.ImageService;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile fileStream);
    string GetImageUrl(string keyName);
    Task DeleteImageAsync(string keyName);
}

