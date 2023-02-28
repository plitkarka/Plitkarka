using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plitkarka.Infrastructure.Services.ImageService.Service;

public interface IImageService
{
    public Task<int> UploadAnImageAsync(string path, string contentType, IAmazonS3 _client);
    public string DownloadAnImageAsync(string keyName, IAmazonS3 _client);
    public Task<bool> DeleteAnImageAsync(string keyName, IAmazonS3 _client);

}

