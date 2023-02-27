namespace Plitkarka.Domain.Services.Encryption;

public interface IEncryptionService
{
    string Hash(string toHash);
    string GenerateSalt();
    bool CheckEquality(string firtsHash, string secondHash);
}
