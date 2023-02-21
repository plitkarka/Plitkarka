using System.Security.Cryptography;
using System.Text;

namespace Plitkarka.Domain.Services.Encryption;

public class Sha256EncryptionService : IEncryptionService
{
    private SHA256 _sha256dEncoder;

    public Sha256EncryptionService()
    {
        _sha256dEncoder = SHA256.Create();
    }

    public bool CheckEquality(string firtsHash, string secondHash)
    {
        return SHA256.Equals(firtsHash, secondHash);
    }

    public string GenerateSalt()
    {
        return DateTime.Now.ToString();
    }

    public string Hash(string toHash)
    {
        if (toHash is null)
            throw new ArgumentNullException(nameof(toHash));

        var bytes = Encoding.Default.GetBytes(toHash);

        var hash = _sha256dEncoder.ComputeHash(bytes);

        return Convert.ToHexString(hash);
    }
}
