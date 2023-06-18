using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record RefreshToken : LogicModel
{
    public string Token { get; set; }

    public DateTime Expires { get; set; }
}
