using Amazon.S3.Model;
using Amazon.S3;
using Plitkarka.Infrastructure.Services.ImageService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Plitkarka.Infrastructure.Services.ImageService.Models;

public record S3Image : IImageService
{
    private string bucketName { get; set; } = "pltkrtdemobucket";
    private List<String> imageExtension;
    //private IAmazonS3 _client;
    public S3Image() { imageExtension = new List<String>() { ".jpg", ".png", ".webp", ".jpeg" }; }
    public S3Image(string bucketName//,IAmazonS3 client
                                      )
    {
        this.bucketName = bucketName;
        
        //_client = client;
    }

    public async Task<int> UploadAnImageAsync(string path, string contentType, IAmazonS3 _client)
    {
 
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception(nameof(path));
        }
        if (!imageExtension.Contains(Path.GetExtension(path)))
        {
            throw new Exception(nameof(path));
        }
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = Path.GetFileName(path),
                FilePath = path,
                ContentType = contentType
            };

            PutObjectResponse response = await _client.PutObjectAsync(putRequest);
            return ((int)response.HttpStatusCode);
        }
        catch (AmazonS3Exception e)
        {

        }
        return 404;
    }

    public string DownloadAnImageAsync(string keyName,IAmazonS3 _client)
    {
        if (string.IsNullOrEmpty(keyName))
        {
            throw new Exception(nameof(keyName));
        }
        string urlString = "";
        try
        {
            GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = keyName,
                Expires = DateTime.UtcNow.AddYears(2),
            };
            urlString =  _client.GetPreSignedURL(request1);
           
        }
        catch (AmazonS3Exception e)
        {
            
        }
        return urlString;
    }


    public async Task<bool> DeleteAnImageAsync(string keyName, IAmazonS3 _client)
    {
        if (string.IsNullOrEmpty(keyName))
        {
            throw new Exception(nameof(keyName));
        }
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            await _client.DeleteObjectAsync(deleteObjectRequest);
            return true;
        }
        catch (AmazonS3Exception e)
        {

        }
        return false;
    }

}