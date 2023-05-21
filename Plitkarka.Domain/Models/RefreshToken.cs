using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record RefreshToken : ActivatedLogicModel
{
    public string Token { get; set; }

    public DateTime Expires { get; set; }
}
