﻿using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Plitkarka.Commons.Configuration;
using Plitkarka.Commons.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Services;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Domain.Services.ImageService;

public record S3Image : IImageService
{
    private static readonly List<string> _imageExtension = new List<string>() { ".jpg", ".png", ".webp", ".jpeg" };
    private static readonly int _urlExpiresMinutes = 10;

    private S3Configuration _s3Configuration { get; init; }
    private IRepository<ImageEntity> _repository { get; init; }

    public S3Image(
        IOptions<S3Configuration> s3Configuration,
        IRepository<ImageEntity> repository) 
    {
        _s3Configuration = s3Configuration.Value;
        _repository = repository;
    }

    public async Task<Guid> UploadImageAsync(IFormFile fileStream)
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

        ImageEntity entity = new ImageEntity()
        {
            ImageId = key,
        };

        var imageId = await _repository.AddAsync(entity);

        return imageId;
    }

    public string DownloadImage(string keyName)
    {
        if (string.IsNullOrEmpty(keyName))
        {
            throw new Exception(nameof(keyName));
        }

        try
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
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

        ImageEntity? toDelete;

        try
        {
            toDelete = await _repository.GetAll()
                .FirstOrDefaultAsync(i => i.ImageId == keyName);
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (toDelete == null)
        {
            throw new ValidationException("Image does not exist");
        }
    }
}
