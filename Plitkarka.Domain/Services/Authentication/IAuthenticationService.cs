using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Services.Authentication;

public interface IAuthenticationService
{
    Task<TokenPair> Authenticate(User toAuthenticate);
}
