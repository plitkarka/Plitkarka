using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Plitkarka.Commons.Configuration;
using Plitkarka.Commons.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Models.Abstractions;
using Org.BouncyCastle.Crypto.Tls;

namespace Plitkarka.Domain.Services.ImageService;

public record S3Image: IImageService
{
    private static readonly List<string> _imageExtension = new List<string>() { ".jpg", ".png", ".webp", ".jpeg" };
    private static readonly int _urlExpiresMinutes = 10;

    private S3Configuration _s3Configuration { get; init; }

    public S3Image(
        IOptions<S3Configuration> s3Configuration) 
    {
        _s3Configuration = s3Configuration.Value;
    }

    public async Task<string> UploadImageAsync(IFormFile fileStream)
    {
        if (fileStream.Length<=0)
        {
            throw new S3ServiceException("No file to load");
        }

        var extension = Path.GetExtension(fileStream.FileName);

        if (!_imageExtension.Contains(extension))
        {
            throw new ValidationException($"Wrong file extension: {nameof(fileStream)}");
        }

        var key = Guid.NewGuid().ToString();

        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _s3Configuration.BucketName,
                Key = key,
                InputStream = fileStream.OpenReadStream(),
                ContentType = fileStream.ContentType,
                Metadata =
                {
                    ["x-amz-meta-originalname"] = fileStream.FileName,
                    ["x-amz-meta-extension"] = extension,

                }
            };

            await _s3Configuration.GetClient().PutObjectAsync(putRequest);
        }
        catch (S3ServiceException ex)
        {
            throw new S3ServiceException(ex.Message);
        }

        return key;
    }

    public string GetImageUrl(string keyName)
    {
        if (string.IsNullOrEmpty(keyName))
        {
            throw new Exception(nameof(keyName));
        }

        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _s3Configuration.BucketName,
                Key = keyName,
                Expires = DateTime.UtcNow.AddMinutes(_urlExpiresMinutes),
            };

            var urlString = _s3Configuration.GetClient().GetPreSignedURL(request);

            return urlString;

        }
        catch (S3ServiceException ex)
        {
            throw new S3ServiceException(ex.Message);
        }
    }

    public async Task DeleteImageAsync(string keyName)
    {
        if (string.IsNullOrEmpty(keyName))
        {
            throw new Exception(nameof(keyName));
        }

        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _s3Configuration.BucketName,
                Key = keyName,
            };

            await _s3Configuration.GetClient().DeleteObjectAsync(request);
        }
        catch (S3ServiceException ex)
        {
            throw new S3ServiceException(ex.Message);
        }
    }
}
