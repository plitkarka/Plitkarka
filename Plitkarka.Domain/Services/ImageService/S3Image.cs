using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Plitkarka.Commons.Configuration;
using Plitkarka.Commons.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Logger;

namespace Plitkarka.Domain.Services.ImageService;

public record S3Image : IImageService
{
    private S3Configuration _s3Configuration { get; init; }
    private IRepository<ImageEntity> _repository { get; init; }
    private ILogger<S3Image> _logger { get; init; }

    private static readonly List<string> _imageExtension = new List<string>() { ".jpg", ".png", ".webp", ".jpeg" };

    private static readonly int _urlExpiresTime = 1;

    public S3Image(
        IOptions<S3Configuration> s3Configuration,
        IRepository<ImageEntity> repository,
        ILogger<S3Image> logger) 
    {
        _s3Configuration = s3Configuration.Value;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> UploadImageAsync(IFormFile fileStream, string contentType)
    {
        if (fileStream.Length<=0)
        {
            throw new Exception(nameof(fileStream));
        }
        if (!_imageExtension.Contains(Path.GetExtension(fileStream.FileName)))
        {
            throw new Exception("Wrong file extension: "+nameof(fileStream));
        }

        var key = Guid.NewGuid().ToString();

        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _s3Configuration.BucketName,
                Key = key,
                InputStream = fileStream.OpenReadStream(),
                ContentType = contentType
            };
            await _s3Configuration.GetClient().PutObjectAsync(putRequest);
           
        }
        catch (S3ServiceException ex)
        {
            throw new S3ServiceException(ex.Message);
        }

        ImageEntity entity = new ImageEntity();
        entity.ImageId = key;
        entity.IsActive = true;
        return await _repository.AddAsync(entity);
    }

    public string DownloadImage(string keyName)
    {
        if (string.IsNullOrEmpty(keyName))
        {
            throw new Exception(nameof(keyName));
        }
        string urlString = "";
        try
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = _s3Configuration.BucketName,
                Key = keyName,
                Expires = DateTime.UtcNow.AddDays(_urlExpiresTime),
            };
            urlString = _s3Configuration.GetClient().GetPreSignedURL(request);

        }
        catch (S3ServiceException ex)
        {
            throw new S3ServiceException(ex.Message);
        }
        return urlString;
    }


    public async Task DeleteImageAsync(string keyName)
    {
        if (string.IsNullOrEmpty(keyName))
        {
            throw new Exception(nameof(keyName));
        }

        ImageEntity? toDelete;

        try
        {
            toDelete = await _repository.GetAll()
                .FirstOrDefaultAsync(i => i.ImageId == keyName);

            if(toDelete == null)
                throw new ValidationException("Image does not exist");

            await _repository.DeleteAsync(toDelete);

        }
        catch (Exception ex)
        {
            _logger.LogDatabaseError($"{nameof(S3Image)}.{nameof(DeleteImageAsync)}", ex.Message);
            throw new MySqlException(ex.Message);
        }

    }

}