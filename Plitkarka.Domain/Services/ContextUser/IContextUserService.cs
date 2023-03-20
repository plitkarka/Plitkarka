using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Services.ContextUser;

public interface IContextUserService
{
    User User { get; set; }
}