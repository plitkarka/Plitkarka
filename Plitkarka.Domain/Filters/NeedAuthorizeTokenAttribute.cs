namespace Plitkarka.Domain.Filters;

/// <summary>
/// Made for actions where user should not be authorized but Authorization toke needed. Such as "RefreshTokenPair"
/// </summary>
public class NeedAuthorizeTokenAttribute : Attribute
{
}
