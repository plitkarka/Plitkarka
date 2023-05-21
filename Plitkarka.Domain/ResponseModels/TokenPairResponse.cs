namespace Plitkarka.Domain.ResponseModels;

/// <summary>
/// Describe result of getting JwtTokens
/// </summary>
public class TokenPairResponse
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}
