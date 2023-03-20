namespace Plitkarka.Domain.Services.Authorization;

public interface IAuthorizationService
{
    Guid Authorize(string token, bool validateTime = true);
}
