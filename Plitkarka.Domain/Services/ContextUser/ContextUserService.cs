using Microsoft.AspNetCore.Http;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Services.ContextUser;

public class ContextUserService : IContextUserService
{
    private static readonly string UserItem = "User";

    private IHttpContextAccessor _accessor { get; init; }

    public ContextUserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public User User
    {
        get
        {
            var user = _accessor.HttpContext.Items[UserItem];

            return (user as User)!;
        }
        set
        {
            _accessor.HttpContext.Items[UserItem] = value;
        }
    }
}
