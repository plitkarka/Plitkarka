namespace Plitkarka.Commons.Configuration;

public class AuthorizationConfiguration
{
    public string SecretKey { get; set; }
    public int TokenMinutesLifetime { get; set; }
}
