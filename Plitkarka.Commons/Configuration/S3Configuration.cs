using Amazon;
using Amazon.S3;

namespace Plitkarka.Commons.Configuration;

public record S3Configuration
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }

    private IAmazonS3? _client = null;

    public IAmazonS3 GetClient()
    {
        if (_client == null)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(AccessKey, SecretKey);
            _client = new AmazonS3Client(awsCredentials, RegionEndpoint.EUCentral1);
        }
        return _client;
    }
}
