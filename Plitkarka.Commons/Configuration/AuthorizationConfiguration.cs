namespace Plitkarka.Commons.Configuration;

public class AuthorizationConfiguration
{
    public string SecretKey { get; set; }
    public int AccessTokenMinutesLifetime { get; set; }
    public int RefreshTokenDaysLifetime { get; set; }
}
