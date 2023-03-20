using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Services.ContextUser;

namespace Plitkarka.Domain.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.RequestServices
            .GetRequiredService<IContextUserService>().User;

        if (user == null)
        {
            throw new UnauthorizedUserException("Authorization required to access this resource");
        }
    }
}
