using Plitkarka.Domain.Models;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Services.Authentication;

public interface IAuthenticationService
{
    Task<TokenPair> Authenticate(User toAuthenticate);
}
